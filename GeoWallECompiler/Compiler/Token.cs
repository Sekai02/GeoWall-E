namespace GeoWallECompiler;

/// <summary>
/// Clase para representar los Token construidos en el Scanner(Lexer)
/// </summary>
public class Token
{
    /// <summary>
    /// Enum que representa el tipo del Token
    /// </summary>
    public readonly TokenType type;
    /// <summary>
    /// Lexema del Token(nombre)
    /// </summary>
    public readonly string lexeme;
    /// <summary>
    /// Literal del Token(valor)
    /// </summary>
    public readonly object literal;
    /// <summary>
    /// Linea en que se encuentra el Token
    /// </summary>
    public readonly int line;

    /// <summary>
    /// Constructor de la clase Token
    /// </summary>
    /// <param name="type"></param>
    /// <param name="lexeme"></param>
    /// <param name="literal"></param>
    /// <param name="line"></param>
    public Token(TokenType type, string lexeme, object literal, int line)
    {
        this.type = type;
        this.lexeme = lexeme;
        this.literal = literal;
        this.line = line;
    }

    /// <summary>
    /// Sobrecarga del metodo ToString() para Token
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return type + " " + lexeme + " " + literal;
    }
}