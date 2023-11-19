namespace GeoWallECompiler;
public class LiteralString : GSharpExpression
{
    public LiteralString(GSharpString @string) => String = @string;
    public GSharpString String { get; }
    public override T Accept<T>(IExpressionVisitor<T> visitor) => throw new NotImplementedException();
}