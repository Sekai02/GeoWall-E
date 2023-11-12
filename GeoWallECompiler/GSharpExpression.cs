namespace GeoWallECompiler;

public abstract class Statement
{
    public abstract T Accept<T>(IStatementVisitor<T> visitor);
}
/// <summary>
/// Clase abstracta de la que heredaran todas las expresiones del lenguaje G#
/// </summary>
public abstract class GSharpExpression : Statement
{

}
/// <summary>
/// Tipos del lenguaje G#
/// </summary>
public enum GSharpTypes 
{
    //aqui se iran añadiendo los tipos que definamos
    GNumber,
    GString,
    GSequence,
    GObject,
    Undetermined
}