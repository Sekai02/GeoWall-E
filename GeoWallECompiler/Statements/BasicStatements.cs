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
    public DrawStatement(GSharpExpression expression, LiteralString? stringExpression)
    {
        Expression = expression;
        StringExpression = stringExpression;
    }
    public GSharpExpression Expression { get; }
    public LiteralString? StringExpression { get; }

    public override void Accept(IStatementVisitor visitor) => visitor.VisitDrawStatement(this);
}
public class PrintStatement : Statement
{
    public PrintStatement(GSharpExpression expression, LiteralString? stringExpression)
    {
        Expression = expression;
        StringExpression = stringExpression;
    }

    public GSharpExpression Expression { get; }
    public LiteralString? StringExpression { get; }
    public override void Accept(IStatementVisitor visitor) => visitor.VisitPrintStatement(this);
}
public class ColorStatement : Statement
{
    public ColorStatement(string color) => Color = color;
    public string Color { get; }
    public override void Accept(IStatementVisitor visitor) => visitor.VisitColorStatent(this);
}
public class Restore : Statement
{
    public override void Accept(IStatementVisitor visitor) => visitor.VisitRestoreStatement(this);
}
