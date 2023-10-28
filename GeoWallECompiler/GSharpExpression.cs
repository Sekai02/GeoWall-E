namespace GeoWallECompiler;

public abstract class GSharpExpression
{
    public abstract GSharpObject GetValue(); // en algun momento el tipo de retorno Object se cambiara por G#Object o algo asi
    public abstract GSharpTypes CheckType();
}
public enum GSharpTypes 
{
    //aqui se iran añadiendo los tipos que definamos
    GNumber,
    GString,
    GObject,
    Undetermined
}