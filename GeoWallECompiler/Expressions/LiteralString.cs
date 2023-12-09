namespace GeoWallECompiler;
public class LiteralString : GSharpExpression
{
    public LiteralString(GString @string) => String = @string;
    public GString String { get; }
    public override T Accept<T>(IExpressionVisitor<T> visitor) => throw new NotImplementedException();
}