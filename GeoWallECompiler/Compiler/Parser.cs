using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using GeoWallECompiler.Expressions;

namespace GeoWallECompiler;

public class Parser
{
    private readonly List<Token> tokens;
    private int current = 0;
    private int checkPoint = NotPresent;

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
        this.checkPoint = -1;
    }

    public List<Statement> Parse()
    {
        List<Statement> statements = new List<Statement>();
        while (!IsAtEnd())
        {
            try
            {
                statements.Add(ParseDeclaration(false));
            }
            catch (GSharpException error)
            {
                ErrorHandler.AddError(error);
                Synchronize();
            }
        }

        return statements;
    }

    private Statement ParseDeclaration(bool isInsideLet)
    {
        if (!Check(TokenType.IDENTIFIER) || !TryFindBefore(TokenType.EQUAL, TokenType.INSTRUCTION_SEPARATOR)) return ParseStatement();
        if (TryFindBefore(TokenType.LEFT_PAREN, TokenType.EQUAL) && !isInsideLet) return ParseFunctionDeclaration();
        return ParseConstantDeclaration();
    }

    private Statement ParseConstantDeclaration()
    {
        List<string> variableNames = new List<string>();
        do
        {
            variableNames.Add(Consume(TokenType.IDENTIFIER, "Expect constant name.").lexeme);
        } while (Match(TokenType.COMMA));

        Consume(TokenType.EQUAL, "Expect '=' after identifier");
        GSharpExpression value = ParseExpression();
        Consume(TokenType.INSTRUCTION_SEPARATOR, "Expect ';' after expression.");
        return new ConstantsDeclaration(variableNames, value);
    }

    private Statement ParseFunctionDeclaration()
    {
        string name = Consume(TokenType.IDENTIFIER, "Expect function name").lexeme;
        Consume(TokenType.LEFT_PAREN, "Expect '(' after '='");
        List<string> parameters = new List<string>();
        if (!Check(TokenType.RIGHT_PAREN))
        {
            do
            {
                if (parameters.Count >= 255)
                {
                    throw new DefaultError("Can't have more than 255 parameters.");
                }

                parameters.Add(Consume(TokenType.IDENTIFIER, "Expect parameter name.").lexeme);
            }
            while (Match(TokenType.COMMA));
        }
        Consume(TokenType.RIGHT_PAREN, "Expect ')' after parameters.");
        Consume(TokenType.EQUAL, "Expect '=' after ')'.");

        GSharpExpression body = ParseExpression();
        Consume(TokenType.INSTRUCTION_SEPARATOR, "Expect ';' after expression.");
        return new FunctionDeclaration(name, parameters, body);
    }

    private GSharpExpression ParseExpression()
    {
        if (Match(TokenType.LET)) return ParseLetStatement();
        if (Match(TokenType.IF)) return ParseIfStatement();
        return ParseLogicalOr();
    }

    private Statement ParseExpressionStatement()
    {
        GSharpExpression expression = ParseExpression();
        Consume(TokenType.INSTRUCTION_SEPARATOR, "Expect ';' after expression.");
        return new ExpressionStatement(expression);
    }

    private GSharpExpression ParseLetStatement()
    {
        List<Statement> expressions = new List<Statement>();
        while (!Check(TokenType.IN) && !IsAtEnd())
        {
            expressions.Add(ParseDeclaration(true));
            // Consume(TokenType.INSTRUCTION_SEPARATOR, "Expect ';' after statement.");
        }

        Consume(TokenType.IN, "Expect 'in' keyword after statements.");

        GSharpExpression expr = ParseExpression();

        return new LetIn(expressions, expr);
    }

    private GSharpExpression ParseIfStatement()
    {
        //Consume(TokenType.LEFT_PAREN, "Expect '(' after 'if'.");
        GSharpExpression condition = ParseExpression();
        //Consume(TokenType.RIGHT_PAREN, "Expect ')' after if condition.");
        Consume(TokenType.THEN, "Expect 'then' after condition.");
        GSharpExpression thenBranch = ParseExpression();
        GSharpExpression elseBranch = null!;
        if (Match(TokenType.ELSE)) elseBranch = ParseExpression();
        return new IfThenElse(condition, thenBranch, elseBranch);
    }

    private GSharpExpression ParseLogicalAnd()
    {
        GSharpExpression expr = ParseEquality();
        while (Match(TokenType.AND))
        {
            GSharpExpression right = ParseEquality();
            expr = new Conjunction(expr, right);
        }

        return expr;
    }

    private GSharpExpression ParsePrimary()
    {
        if (Match(TokenType.NUMBER, TokenType.STRING, TokenType.LEFT_BRACE))
        {
            Token previous = Previous();
            switch (previous.type)
            {
                case TokenType.NUMBER:
                    return new LiteralNumber(new GSNumber((double)previous.literal));
                case TokenType.STRING:
                    return new LiteralString(new GString((string)previous.literal));
                case TokenType.LEFT_BRACE:
                    bool ellipsisBetween = TryFindBefore(TokenType.ELLIPSIS, TokenType.RIGHT_BRACE);

                    if (ellipsisBetween)
                    {
                        GSharpExpression leftBound = ParseExpression();
                        Consume(TokenType.ELLIPSIS, "Expect '...' after left bound of sequence");
                        if (!Check(TokenType.RIGHT_BRACE))
                        {
                            GSharpExpression rightBound = ParseExpression();
                            Consume(TokenType.RIGHT_BRACE, "Expect '}' after right bound of sequence");
                            return new FiniteRangeExpression(leftBound, rightBound);
                        }
                        Consume(TokenType.RIGHT_BRACE, "Expect '}' after right bound of sequence");
                        return new InfiniteRangeExpression(leftBound);
                    }
                    else
                    {
                        List<GSharpExpression> sequenceValues = new List<GSharpExpression>();
                        if (!Check(TokenType.RIGHT_BRACE))
                        {
                            do
                            {
                                sequenceValues.Add(ParseExpression());
                            }
                            while (Match(TokenType.COMMA));
                        }
                        Consume(TokenType.RIGHT_BRACE, "Expect '}' after sequence");
                        return new LiteralArrayExpression(sequenceValues);
                    }
            }
        }

        if (Match(TokenType.IDENTIFIER))
        {
            string name = Previous().lexeme;
            if (Match(TokenType.LEFT_PAREN))
            {
                List<GSharpExpression> arguments = new List<GSharpExpression>();
                if (!Check(TokenType.RIGHT_PAREN))
                {
                    do
                    {
                        arguments.Add(ParseExpression());
                    }
                    while (Match(TokenType.COMMA));
                }
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after arguments");
                //GSharpExpression body = ParseExpression();

                return new FunctionCall(name, arguments);
            }

            return new Constant(name);
        }

        if (Match(TokenType.LEFT_PAREN))
        {
            GSharpExpression expr = ParseExpression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
            return new Grouping(expr);
        }

        throw new DefaultError("Expect expression.");
    }

    private GSharpExpression ParseUnary()
    {
        if (Match(TokenType.NOT, TokenType.MINUS))
        {
            Token oper = Previous();
            GSharpExpression right = ParseUnary();
            switch (oper.type)
            {
                case TokenType.NOT:
                    return new Negation(right);
                case TokenType.MINUS:
                    return new Negative(right);
            }
        }

        return ParsePrimary();
    }

    private GSharpExpression ParseFactor()
    {
        GSharpExpression expr = ParseUnary();
        while (Match(TokenType.DIVISION, TokenType.PRODUCT, TokenType.MOD))
        {
            Token oper = Previous();
            GSharpExpression right = ParseUnary();
            switch (oper.type)
            {
                case TokenType.DIVISION:
                    expr = new Division(expr, right);
                    break;
                case TokenType.PRODUCT:
                    expr = new Multiplication(expr, right);
                    break;
                case TokenType.MOD:
                    expr = new Module(expr, right);
                    break;
            }
        }

        return expr;
    }

    private GSharpExpression ParseTerm()
    {
        GSharpExpression expr = ParseFactor();
        while (Match(TokenType.MINUS, TokenType.PLUS))
        {
            Token oper = Previous();
            GSharpExpression right = ParseFactor();
            switch (oper.type)
            {
                case TokenType.MINUS:
                    expr = new Subtraction(expr, right);
                    break;
                case TokenType.PLUS:
                    expr = new Addition(expr, right);
                    break;
            }
        }

        return expr;
    }

    private GSharpExpression ParseComparison()
    {
        GSharpExpression expr = ParseTerm();
        while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
        {
            Token oper = Previous();
            GSharpExpression right = ParseTerm();
            switch (oper.type)
            {
                case TokenType.GREATER:
                    expr = new GreaterThan(expr, right);
                    break;
                case TokenType.GREATER_EQUAL:
                    expr = new GreaterEqualThan(expr, right);
                    break;
                case TokenType.LESS:
                    expr = new LowerThan(expr, right);
                    break;
                case TokenType.LESS_EQUAL:
                    expr = new LowerEqualThan(expr, right);
                    break;
            }
        }

        return expr;
    }

    private GSharpExpression ParseEquality()
    {
        GSharpExpression expr = ParseComparison();
        while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
        {
            Token oper = Previous();
            GSharpExpression right = ParseComparison();
            switch (oper.type)
            {
                case TokenType.BANG_EQUAL:
                    expr = new UnEqual(expr, right);
                    break;
                case TokenType.EQUAL_EQUAL:
                    expr = new Equal(expr, right);
                    break;
            }
            //expr = oper.type == TokenType.BANG_EQUAL ? new UnEqual(expr, right) : new Equal(expr, right);
        }

        return expr;
    }

    private GSharpExpression ParseLogicalOr()
    {
        GSharpExpression expr = ParseLogicalAnd();
        while (Match(TokenType.OR))
        {
            GSharpExpression right = ParseEquality();
            expr = new Disjunction(expr, right);
        }

        return expr;
    }

    private Statement ParseColorStatement()
    {
        if (Match(TokenType.IDENTIFIER))
        {
            string colorName = Previous().lexeme;
            Consume(TokenType.INSTRUCTION_SEPARATOR, "Expect ';' after color name.");
            return new ColorStatement(colorName);
        }
        throw new Error(ErrorType.SYNTAX_ERROR, "Expect color name.");
    }

    private Statement ParseDrawStatement()
    {
        GSharpExpression expressionToDraw = ParseExpression();
        if (Check(TokenType.INSTRUCTION_SEPARATOR))
        {
            Consume(TokenType.INSTRUCTION_SEPARATOR, "Expect ';' after identifier");
            return new DrawStatement(expressionToDraw, null!);
        }
        string label = Consume(TokenType.STRING, "Expect string after identifier.").lexeme;
        Consume(TokenType.INSTRUCTION_SEPARATOR, "Expect ';' after string");
        return new DrawStatement(expressionToDraw, new LiteralString(new GString(label)));
    }

    private Statement ParsePrintStatement()
    {
        GSharpExpression expressionToPrint = ParseExpression();
        Consume(TokenType.INSTRUCTION_SEPARATOR, "Expect ';' after string");
        return new PrintStatement(expressionToPrint);
    }

    private Statement ParseImportStatement()
    {
        string libraryName = Consume(TokenType.STRING, "Expect string after import statement.").lexeme;
        Consume(TokenType.INSTRUCTION_SEPARATOR, "Expect ';' after string");
        return new Import(libraryName);
    }

    private Statement ParseReceiverStatement(TokenType type)
    {
        if (Check(TokenType.SEQUENCE))
        {
            Consume(TokenType.SEQUENCE, "Expect 'sequence' after type.");
            string name = Consume(TokenType.IDENTIFIER, "Expect sequence name.").lexeme;
            Consume(TokenType.INSTRUCTION_SEPARATOR, "Expect ';' after name.");
            return new Reciever(InferType(type), name, true);
        }
        else
        {
            string name = Consume(TokenType.IDENTIFIER, "Expect name.").lexeme;
            Consume(TokenType.INSTRUCTION_SEPARATOR, "Expect ';' after name.");
            return new Reciever(InferType(type), name, false);
        }
    }

    private Statement ParseStatement()
    {
        if (Match(TokenType.RESTORE))
        {
            Consume(TokenType.INSTRUCTION_SEPARATOR, "Expect ';' after statement");
            return new Restore();
        }
        if (Match(TokenType.COLOR)) return ParseColorStatement();
        if (Match(TokenType.DRAW)) return ParseDrawStatement();
        if (Match(TokenType.IMPORT)) return ParseImportStatement();
        if (Match(TokenType.PRINT)) return ParsePrintStatement();
        if (Match(TokenType.POINT, TokenType.LINE, TokenType.SEGMENT, TokenType.RAY,
        TokenType.CIRCLE, TokenType.ARC)) return ParseReceiverStatement(Previous().type);

        return ParseExpressionStatement();
    }

    private Token Peek() => tokens[current];
    private bool IsAtEnd() => Peek().type == TokenType.EOF;
    private Token Previous() => tokens[current - 1];

    private bool Match(params TokenType[] types)
    {
        foreach (TokenType type in types)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
        }

        return false;
    }

    private Token Consume(TokenType type, String message)
    {
        if (Check(type)) return Advance();

        throw new DefaultError(message, "syntax", Scanner.Line);
    }

    private bool Check(TokenType type)
    {
        if (IsAtEnd()) return false;
        return Peek().type == type;
    }

    private bool TryFind(TokenType type)
    {
        for (int i = current; i < tokens.Count; i++)
        {
            if (IsAtEnd()) return false;
            if (tokens[i].type == TokenType.INSTRUCTION_SEPARATOR) return false;
            if (tokens[i].type == type) return true;
        }
        return false;
    }

    private bool TryFindBefore(TokenType target, TokenType breakpoint)
    {
        for (int i = current; i < tokens.Count; i++)
        {
            if (IsAtEnd()) return false;
            if (tokens[i].type == TokenType.INSTRUCTION_SEPARATOR) return false;
            if (tokens[i].type == breakpoint) return false;
            if (tokens[i].type == target) return true;
        }
        return false;
    }

    private Token Advance()
    {
        if (!IsAtEnd()) current++;
        return Previous();
    }

    private void Synchronize()
    {
        Advance();

        while (!IsAtEnd())
        {
            if (Previous().type == TokenType.INSTRUCTION_SEPARATOR) return;
            Advance();
        }
    }

    private GTypeNames InferType(TokenType type)
    {
        switch (type)
        {
            case TokenType.LINE:
                return GTypeNames.Line;
            case TokenType.ARC:
                return GTypeNames.Arc;
            case TokenType.CIRCLE:
                return GTypeNames.Circle;
            case TokenType.RAY:
                return GTypeNames.Ray;
            case TokenType.POINT:
                return GTypeNames.Point;
            case TokenType.SEGMENT:
                return GTypeNames.Segment;
            default:
                return GTypeNames.Undetermined;
        }
    }

    private const int NotPresent = -1;
}