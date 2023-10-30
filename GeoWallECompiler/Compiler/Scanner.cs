using System.Collections;
using System.Dynamic;
using System.Security.Principal;

namespace GeoWallECompiler;
public class Scanner
{
    private readonly string source;
    private readonly List<Token> tokens = new List<Token>();
    private int start = 0;
    private int current = 0;
    public static int Line = 0;

    private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>(){
        {"if",TokenType.IF},
        {"else",TokenType.ELSE},
        {"then",TokenType.THEN},
        {"let",TokenType.LET},
        {"in",TokenType.IN},

        {"draw",TokenType.DRAW},
        {"color",TokenType.COLOR},
        {"restore",TokenType.RESTORE},
        {"import",TokenType.IMPORT},

        //Temporally removed

        /*{"point",TokenType.POINT},
        {"Line",TokenType.LINE},
        {"segment",TokenType.SEGMENT},
        {"ray",TokenType.RAY},
        {"circle",TokenType.CIRCLE},
        {"arc",TokenType.ARC},*/

        {"undefined",TokenType.UNDEFINED},
        {"PI",TokenType.PI},
        {"E",TokenType.EULER}
    };

    public Scanner(string source)
    {
        Line++;
        this.source = source;
    }

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

    private void ScanToken()
    {
        char c = Advance();
        switch (c)
        {
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case '{': AddToken(TokenType.LEFT_PAREN); break;
            case '}': AddToken(TokenType.RIGHT_PAREN); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '+': AddToken(TokenType.PLUS); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '*': AddToken(TokenType.PRODUCT); break;
            case '^': AddToken(TokenType.POWER); break;
            case '/': AddToken(TokenType.DIVISION); break;
            case '%': AddToken(TokenType.MOD); break;
            case '&': AddToken(TokenType.AND); break;
            case '|': AddToken(TokenType.OR); break;

            case '!':
                AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
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
                    Error err = new Error(ErrorType.LEXICAL_ERROR, "Unexpected character.", Line);
                    err.Report();
                }
                break;
        }
    }

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

    private bool IsAlpha(char c)
    {
        return ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_');
    }

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

    private void _String()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n') Line++;
            Advance();
        }

        if (IsAtEnd())
        {
            Error err = new Error(ErrorType.LEXICAL_ERROR, "Unterminated string.", Line);
            err.Report();
            return;
        }

        Advance();

        string value = Substring(source, start + 1, current - 1);
        AddToken(TokenType.STRING, value);
    }

    private bool Match(char expected)
    {
        if (IsAtEnd()) return false;
        if (source[current] != expected) return false;

        current++;
        return true;
    }

    private char Peek()
    {
        if (IsAtEnd()) return '\0';
        return source[current];
    }

    private char PeekNext()
    {
        if (current + 1 >= source.Length) return '\0';
        return source[current + 1];
    }

    private bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    private bool IsAtEnd()
    {
        return current >= source.Length;
    }

    private char Advance()
    {
        return source[current++];
    }

    private string Substring(string s, int begIdx, int endIdx)
    {
        return s.Substring(begIdx, endIdx - begIdx);
    }

    private void AddToken(TokenType type)
    {
        AddToken(type, null!);
    }

    private void AddToken(TokenType type, object literal)
    {
        string text = Substring(source, start, current);
        tokens.Add(new Token(type, text, literal, Line));
    }
}