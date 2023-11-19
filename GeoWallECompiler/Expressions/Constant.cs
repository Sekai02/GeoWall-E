namespace GeoWallECompiler;

/// <summary>
/// Representa a las constantes de G#
/// </summary>
public class Constant : GSharpExpression
{
    /// <summary>
    /// Construye un objeto que representa una constante de G#
    /// </summary>
    /// <param name="value">Valor que toma la constante</param>
    public Constant(string name) => Name = name;
    public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitConstant(this);
    /// <summary>
    /// Nombre de la constante
    /// </summary>
    public string Name { get; private set; }
}
