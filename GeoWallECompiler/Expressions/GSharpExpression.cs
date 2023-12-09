namespace GeoWallECompiler;
/// <summary>
/// Clase abstracta de la que heredaran todas las expresiones del lenguaje G#
/// </summary>
public abstract class GSharpExpression
{
    public bool TypeSetted = false;
    private GSharpType types = new(GTypeNames.Undetermined);
    public GSharpType ExpressionType
    {
        get => types;
        set
        {
            if (!TypeSetted)
            {
                types = value;
                TypeSetted = true;
            }
        } 
    }
    public abstract T Accept<T>(IExpressionVisitor<T> visitor);
}
