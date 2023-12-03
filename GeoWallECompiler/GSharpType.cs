namespace GeoWallECompiler;
public class GSharpType
{
    public bool HasGenericType;
    private GTypeNames genericType = GTypeNames.Undetermined;
    public GSharpType(GTypeNames typeName)
    {
        Name = typeName;
        HasGenericType = false;
        SetIsFigureProperty(typeName);
    }
    public GSharpType(GTypeNames typeName, GTypeNames genericType)
    {
        Name = typeName;
        if (Name != GTypeNames.GSequence) { 
            HasGenericType = false;
            SetIsFigureProperty(typeName);
        }
        else
        {
            HasGenericType = true;
            this.genericType = genericType;
            SetIsFigureProperty(genericType);
        }
    }
    private void SetIsFigureProperty(GTypeNames typeNames)
    {
        bool isFigure = typeNames is GTypeNames.Arc
            or GTypeNames.Ray
            or GTypeNames.Point
            or GTypeNames.Segment
            or GTypeNames.Circle
            or GTypeNames.Line;
        IsFigure = isFigure;
    }
    public bool IsFigure { get; private set; }
    public GTypeNames Name { get; }
    public GTypeNames GenericType => HasGenericType ? genericType : throw new Exception("Type does not contain generic type");
    public static Type ConvertToType(GTypeNames gType)
    {
        return gType switch
        {
            GTypeNames.GSequence => typeof(GSequence),
            GTypeNames.GNumber => typeof(GSNumber),
            GTypeNames.GString => typeof(GString),
            GTypeNames.GObject => typeof(GSObject),
            GTypeNames.Point => typeof(GSPoint),
            GTypeNames.Circle => typeof(Circle),
            GTypeNames.Line => typeof(Line),
            GTypeNames.Segment => typeof(Segment),
            GTypeNames.Ray => typeof(Ray),
            GTypeNames.Arc => typeof(Arc),
            GTypeNames.Measure => typeof(Measure),
            _ => typeof(GSObject),
        };
    }
    public override string ToString() => HasGenericType? $"{genericType} sequence": Name.ToString();
    public static bool operator ==(GSharpType a, GSharpType b)
    {
        if(a.Name == b.Name)
            return a.genericType == b.genericType;
        return false;
    }
    public static bool operator !=(GSharpType a, GSharpType b)
    {
        if (a.Name != b.Name)
            return false;
        return a.genericType != b.genericType;
    }
}
/// <summary>
/// Tipos del lenguaje G#
/// </summary>
public enum GTypeNames
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
    Measure,
    Undetermined
}
