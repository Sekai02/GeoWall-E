namespace GeoWallECompiler;
public class ExpressionStatement : Statement
{
    public ExpressionStatement(GSharpExpression expression)
    {
        Expression = expression;
    }
    public GSharpExpression Expression { get; }
    public override void Accept(IStatementVisitor visitor) => visitor.VisitExpressionStatement(this);
}
