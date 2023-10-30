namespace GeoWallECompiler;

/// <summary>
/// Tipos de Error.
/// </summary>
public enum ErrorType
{
    LEXICAL_ERROR,
    SYNTAX_ERROR,
    SEMANTIC_ERROR
}

/// <summary>
/// Clase que representa los errores para el manejo de los mismos.
/// </summary>
public class Error : Exception
{
    /// <summary>
    /// Constructor para la clase Error
    /// </summary>
    /// <param name="type"></param>
    /// <param name="message"></param>
    public Error(ErrorType type, string message)
    {
        this.type = type;
        this.message = message;
        this.token=null!;
        HadError = true;
    }

    /// <summary>
    /// Constructor para la clase Error
    /// </summary>
    /// <param name="type"></param>
    /// <param name="message"></param>
    /// <param name="token"></param>
    public Error(ErrorType type, string message, Token token)
    {
        this.type = type;
        this.message = message;
        this.token = token;

        HadError = true;
    }

    /// <summary>
    /// Constructor para la clase error
    /// </summary>
    /// <param name="type"></param>
    /// <param name="message"></param>
    /// <param name="line"></param>
    public Error(ErrorType type, string message, int line)
    {
        this.type = type;
        this.message = message;
        this.line = line;
        this.token = null!;

        HadError = true;
    }

    /// <summary>
    /// Metodo para imprimir por consola los mensajes
    /// </summary>
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

    /// <summary>
    /// Propiedad estatica que indica si hubo error en el programa o no
    /// </summary>
    public static bool HadError = false;
    readonly ErrorType type;
    readonly string message;
    readonly Token token;
    readonly int line;
}