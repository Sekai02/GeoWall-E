namespace GeoWallECompiler;
/// <summary>
/// Clase abstracta de la que heredaran todas las expresiones del lenguaje G#
/// </summary>
public abstract class GSharpExpression
{
    public abstract T Accept<T>(IExpressionVisitor<T> visitor);
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
    Point,
    Circle,
    Line,
    Segment,
    Ray,
    Arc,
    SegmentSequence,
    PointSequence,
    CircleSequence,
    LineSequence,
    RaySequence,
    ArcSequence,
    Undetermined
}