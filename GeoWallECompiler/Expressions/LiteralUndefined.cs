namespace GeoWallECompiler.Expressions;
public class LiteralUndefined : GSharpExpression
{
    public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitLiteralUndefined(this);
}
