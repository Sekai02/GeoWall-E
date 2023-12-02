namespace GeoWallECompiler;
public class LiteralString : GSharpExpression
{
    public LiteralString(GSString @string) => String = @string;
    public GSString String { get; }
    public override T Accept<T>(IExpressionVisitor<T> visitor) => throw new NotImplementedException();
}