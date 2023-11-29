using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoWallECompiler;
public class GSharpType
{
    public bool HasGenericType;
    private GTypeNames genericType = GTypeNames.Undetermined;
    public GSharpType(GTypeNames typeName)
    {
        Name = typeName;
        HasGenericType = false;
    }
    public GSharpType(GTypeNames typeName, GTypeNames genericType)
    {
        Name = typeName;
        if (Name != GTypeNames.GSequence)
            HasGenericType = false;
        else
        {
            HasGenericType = true;
            this.genericType = genericType;
        }
    }
    public GTypeNames Name { get; }
    public GTypeNames GenericType => HasGenericType ? genericType : throw new Exception("Type does not contain generic type");
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
    Undetermined
}
