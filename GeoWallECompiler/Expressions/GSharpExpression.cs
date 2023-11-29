namespace GeoWallECompiler;
/// <summary>
/// Clase abstracta de la que heredaran todas las expresiones del lenguaje G#
/// </summary>
public abstract class GSharpExpression
{
    public abstract T Accept<T>(IExpressionVisitor<T> visitor);
}
