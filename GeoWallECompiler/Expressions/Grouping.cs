namespace GeoWallECompiler.Expressions;
public class Grouping : GSharpExpression
{
    public Grouping(GSharpExpression expression)
    {
        Expression = expression;
    }
    public GSharpExpression Expression { get; }

    public override T Accept<T>(IExpressionVisitor<T> visitor) => Expression.Accept(visitor);
}
