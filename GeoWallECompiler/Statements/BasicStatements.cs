using System.Drawing;

namespace GeoWallECompiler;
public class ExpressionStatement : Statement
{
    public ExpressionStatement(GSharpExpression expression) => Expression = expression;
    public GSharpExpression Expression { get; }
    public override void Accept(IStatementVisitor visitor) => visitor.VisitExpressionStatement(this);
}
public class DrawStatement : Statement
{
    public DrawStatement(GSharpExpression expression, LiteralString stringExpression)
    {
        Expression = expression;
        StringExpression = stringExpression;
    }
    public GSharpExpression Expression { get; }
    public LiteralString StringExpression { get; }

    public override void Accept(IStatementVisitor visitor) => throw new NotImplementedException();
}
public class ColorStatement : Statement
{
    public ColorStatement(Color color) => Color = color;
    public Color Color { get; }
    public override void Accept(IStatementVisitor visitor) => throw new NotImplementedException();
}
public class Restore : Statement
{
    public override void Accept(IStatementVisitor visitor) => throw new NotImplementedException();
}
