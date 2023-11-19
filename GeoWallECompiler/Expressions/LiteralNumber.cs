namespace GeoWallECompiler;

/// <summary>
/// Representa un valor explicitamente en el codigo. Por ejemplo 3, "hello world", {1,2,3}
/// </summary>
public class LiteralNumber : GSharpExpression
{
    /// <summary>
    /// Construye un objeto que representa un valor literal
    /// </summary>
    /// <param name="value">Objeto valor de la expresion</param>
    public LiteralNumber(GSharpNumber value) => Value = value;
    public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitLiteralNumber(this);
    /// <summary>
    /// Valor del literal
    /// </summary>
    public GSharpNumber Value { get; private set; }
}
