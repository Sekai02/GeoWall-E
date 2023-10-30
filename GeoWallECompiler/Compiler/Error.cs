namespace GeoWallECompiler;

public enum ErrorType
{
    LEXICAL_ERROR,
    SYNTAX_ERROR,
    SEMANTIC_ERROR
}

public class Error : Exception
{
    public Error(ErrorType type, string message)
    {
        this.type = type;
        this.message = message;
        this.token=null!;
        hadError = true;
    }

    public Error(ErrorType type, string message, Token token)
    {
        this.type = type;
        this.message = message;
        this.token = token;

        hadError = true;
    }

    public Error(ErrorType type, string message, int line)
    {
        this.type = type;
        this.message = message;
        this.line = line;
        this.token = null!;

        hadError = true;
    }

    public void Report()
    {
        switch (type)
        {
            case ErrorType.LEXICAL_ERROR:
                LexicalError();
                break;
            case ErrorType.SYNTAX_ERROR:
                SyntaxError();
                break;
            case ErrorType.SEMANTIC_ERROR:
                SemanticError();
                break;
        }
    }

    private void LexicalError()
    {
        if (token == null)
            Console.WriteLine("{0}: {1} [line {2}]", type, message, line);
        else
            Console.WriteLine("{0}: {1} at {2} [line {3}]", type, message, token.lexeme, token.line);
    }

    private void SyntaxError()
    {
        if (token == null)
            Console.WriteLine("{0}: {1} [line {2}]", type, message, line);
        else
            Console.WriteLine("{0}: {1} at {2} [line {3}]", type, message, token.lexeme, token.line);
    }

    private void SemanticError()
    {
        if (token == null)
            Console.WriteLine("{0}: {1} [line {2}]", type, message, line);
        else
            Console.WriteLine("{0}: {1} at {2} [line {3}]", type, message, token.lexeme, token.line);
    }

    public static bool hadError = false;
    readonly ErrorType type;
    readonly string message;
    readonly Token token;
    readonly int line;
}