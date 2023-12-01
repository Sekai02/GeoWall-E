namespace GeoWallECompiler;

public class Scanner
{
    /// <summary>
    /// Codigo a tokenizar
    /// </summary>
    private readonly string source;
    /// <summary>
    /// Lista de Tokens
    /// </summary>
    private readonly List<Token> tokens = new List<Token>();
    /// <summary>
    /// inicio del Token actual
    /// </summary>
    private int start = 0;
    /// <summary>
    /// Posicion del caracter actual que se esta analizando en la linea
    /// </summary>
    private int current = 0;
    private bool isInLet = false;
    /// <summary>
    /// Linea actual en que se encuentra ejecutandose el programa
    /// </summary>
    public static int Line = 0;


    private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>(){
        {"if",TokenType.IF},
        {"else",TokenType.ELSE},
        {"then",TokenType.THEN},
        {"let",TokenType.LET},
        {"in",TokenType.IN},
        {"and", TokenType.AND},
        {"or", TokenType.OR},
        {"not", TokenType.NOT},

        {"draw",TokenType.DRAW},
        {"color",TokenType.COLOR},
        {"restore",TokenType.RESTORE},
        {"import",TokenType.IMPORT},

        //Temporally removed

        {"point",TokenType.POINT},
        {"line",TokenType.LINE},
        {"segment",TokenType.SEGMENT},
        {"ray",TokenType.RAY},
        {"circle",TokenType.CIRCLE},
        {"arc",TokenType.ARC},
        
        {"undefined",TokenType.UNDEFINED},
        {"PI",TokenType.PI},
        {"E",TokenType.EULER}
    };

    /// <summary>
    /// Constructor para escanear una linea
    /// </summary>
    /// <param name="source"></param>
    public Scanner(string source)
    {
        Line++;
        this.source = source;
    }

    /// <summary>
    /// Escanea la linea actual dada en el constructor
    /// </summary>
    /// <returns>List<Token></returns>
    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            start = current;
            ScanToken();
        }

        tokens.Add(new Token(TokenType.EOF, "", null!, Line));
        return tokens;
    }

    /// <summary>
    /// Escanea el Token actual
    /// </summary>
    private void ScanToken()
    {
        char c = Advance();
        switch (c)
        {
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case '{': AddToken(TokenType.LEFT_BRACE); break;
            case '}': AddToken(TokenType.RIGHT_BRACE); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '+': AddToken(TokenType.PLUS); break;
            case '*': AddToken(TokenType.PRODUCT); break;
            case '^': AddToken(TokenType.POWER); break;
            case '/': AddToken(TokenType.DIVISION); break;
            case '%': AddToken(TokenType.MOD); break;

            case ';':
                if (isInLet) AddToken(TokenType.ASSIGN_SEPARATOR);
                else AddToken(TokenType.INSTRUCTION_SEPARATOR);
                break;

            case '.':
                if (Match('.') && Match('.')) AddToken(TokenType.ELLIPSIS);
                else ErrorHandler.AddError(new DefaultError("Only `...` is allowed", "lexical", Line));
                break;
            case '!':
                //AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                if (Match('=')) AddToken(TokenType.BANG_EQUAL);
                else ErrorHandler.AddError(new LexicalError("!", Line));
                break;
            case '=':
                if (Match('=')) AddToken(TokenType.EQUAL_EQUAL);
                else AddToken(TokenType.EQUAL);
                break;
            case '<':
                AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                break;
            case '>':
                AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                break;

            case ' ':
            case '\r':
            case '\t':
                break;

            case '\n':
                Line++;
                break;

            case '"':
                _String();
                break;

            default:
                if (IsDigit(c))
                {
                    Number();
                }
                else if (IsAlpha(c))
                {
                    Identifier();
                }
                else
                {
                    //Error err = new Error(ErrorType.LEXICAL_ERROR, "Unexpected character.", Line);
                    //err.Report();
                    ErrorHandler.AddError(new LexicalError(Peek().ToString(), Line));
                }
                break;
        }
    }

    /// <summary>
    /// Escanea un identificador
    /// </summary>
    private void Identifier()
    {
        while (IsAlphaNumeric(Peek())) Advance();

        string text = Substring(source, start, current);
        TokenType type;
        object literal = null!;
        if (keywords.TryGetValue(text, out _))
        {
            type = keywords[text];

            switch (type)
            {
                case TokenType.LET:
                    isInLet = true;
                    break;
                case TokenType.IN:
                    isInLet = false;
                    break;
                case TokenType.EULER:
                    literal = Math.E;
                    break;
                case TokenType.PI:
                    literal = Math.PI;
                    break;
                default:
                    break;
            }
        }
        else
        {
            type = TokenType.IDENTIFIER;
        }

        if (literal == null) AddToken(type);
        else AddToken(type, literal);
    }

    /// <summary>
    /// Evalua si el caracter es alfabetico(A-Za-z_)
    /// </summary>
    /// <param name="c"></param>
    /// <returns>bool</returns>
    private bool IsAlpha(char c)
    {
        return ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_');
    }

    /// <summary>
    /// Evalua si el caracter es alfanumerico(A-Za-z_0-9)
    /// </summary>
    /// <param name="c"></param>
    /// <returns>bool</returns>
    private bool IsAlphaNumeric(char c)
    {
        return IsAlpha(c) || IsDigit(c);
    }

    private void Number()
    {
        while (IsDigit(Peek())) Advance();

        if (Peek() == '.' && IsDigit(PeekNext()))
        {
            Advance();

            while (IsDigit(Peek())) Advance();
        }

        AddToken(TokenType.NUMBER, Double.Parse(Substring(source, start, current)));
    }

    /// <summary>
    /// Escanea un string
    /// </summary>
    private void _String()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n') Line++;
            Advance();
        }

        if (IsAtEnd())
        {
            //Error err = new Error(ErrorType.LEXICAL_ERROR, "Unterminated string.", Line);
            //err.Report();
            ErrorHandler.AddError(new DefaultError("Unterminated string", "lexical", Line));
            return;
        }

        Advance();

        string value = Substring(source, start + 1, current - 1);
        AddToken(TokenType.STRING, value);
    }

    /// <summary>
    /// Comprueba que el caracter actual matchee con el esperado
    /// </summary>
    /// <param name="expected"></param>
    /// <returns>bool</returns>
    private bool Match(char expected)
    {
        if (IsAtEnd()) return false;
        if (source[current] != expected) return false;

        current++;
        return true;
    }

    /// <summary>
    /// Retorna el caracter actual
    /// </summary>
    /// <returns>char</returns>
    private char Peek()
    {
        if (IsAtEnd()) return '\0';
        return source[current];
    }

    /// <summary>
    /// Retorna el siguiente caracter
    /// </summary>
    /// <returns>char</returns>
    private char PeekNext()
    {
        if (current + 1 >= source.Length) return '\0';
        return source[current + 1];
    }

    /// <summary>
    /// Comprueba si el caracter dado es digito o no
    /// </summary>
    /// <param name="c"></param>
    /// <returns>bool</returns>
    private bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    /// <summary>
    /// Comprueba si se llego al final de la linea
    /// </summary>
    /// <returns>bool</returns>
    private bool IsAtEnd()
    {
        return current >= source.Length;
    }

    /// <summary>
    /// Consume el caracter actual y avanza al siguiente
    /// </summary>
    /// <returns>char</returns>
    private char Advance()
    {
        return source[current++];
    }

    /// <summary>
    /// Funcion util para tomar un substring
    /// </summary>
    /// <param name="s"></param>
    /// <param name="begIdx"></param>
    /// <param name="endIdx"></param>
    /// <returns></returns>
    private string Substring(string s, int begIdx, int endIdx)
    {
        return s.Substring(begIdx, endIdx - begIdx);
    }

    /// <summary>
    /// Añade un Token a la lista de Tokens para luego ser 
    /// usados por el Parser
    /// </summary>
    /// <param name="type"></param>
    private void AddToken(TokenType type)
    {
        AddToken(type, null!);
    }

    /// <summary>
    /// Añade un Token con un valor de literal
    /// </summary>
    /// <param name="type"></param>
    /// <param name="literal"></param>
    private void AddToken(TokenType type, object literal)
    {
        string text = Substring(source, start, current);
        tokens.Add(new Token(type, text, literal, Line));
    }
}