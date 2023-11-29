namespace GeoWallECompiler;

public abstract class LiteralSequence : GSharpExpression
{
    public IEnumerable<GSharpExpression> Expressions { get; protected set; }
    public int Count { get; protected set; }
    public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitLiteralSequence(this);
    public abstract LiteralSequence GetTail(int tailBeggining);
    public GSharpSequence<GSharpObject> GetSequenceValue(Evaluator evaluator)
    {
        List<GSharpObject> values = new(ToGSharpObjectSequence(evaluator));
        return new ArraySequence<GSharpObject>(values);
    }
    private IEnumerable<T> ToGSharpObjectSequence<T>(IExpressionVisitor<T> visitor)
    {
        foreach (GSharpExpression expression in Expressions)
            yield return expression.Accept(visitor);
    }
}
public class LiteralArrayExpression : LiteralSequence
{
    public LiteralArrayExpression(List<GSharpExpression> expressions) 
    {
        Expressions = expressions;
        Count = expressions.Count;
    }
    public override LiteralSequence? GetTail(int tailbeggining)
    {
        List<GSharpExpression> expressions = new(ChoppSequence(tailbeggining));
        return expressions.Count == 0 ? null : new LiteralArrayExpression(expressions);
    }
    private IEnumerable<GSharpExpression> ChoppSequence(int a)
    {
        int count = 0;
        foreach (GSharpExpression expression in Expressions)
        {
            if (count >= a)
                yield return expression;
            count++;
        }
    }    
}
public class FiniteRangeExpression : LiteralSequence
{
    public FiniteRangeExpression(GSharpExpression leftBoundExpression, GSharpExpression rightBoundExpression)
    {
        if (leftBoundExpression is not LiteralNumber || rightBoundExpression is not LiteralNumber)
            throw new Exception();
        Expressions = FiniteRange((LiteralNumber)leftBoundExpression, (LiteralNumber)rightBoundExpression);
        LeftBound = (LiteralNumber)leftBoundExpression;
        RightBound = (LiteralNumber)rightBoundExpression;
        Count = (int)(RightBound.Value.Value - LeftBound.Value.Value);
        ExpressionType = new(GTypeNames.GSequence, GTypeNames.GNumber);
    }
    public LiteralNumber LeftBound { get; }
    public LiteralNumber RightBound { get; }
    private static IEnumerable<GSharpExpression> FiniteRange(LiteralNumber left, LiteralNumber right)
    {
        double start = left.Value.Value;
        double end = right.Value.Value;
        if (!double.IsInteger(start) || !double.IsInteger(end))
            throw new DefaultError("Sequences declaration with '...' can only take integers as limits", "Semantic");
        if (start < end)
            for (int i = (int)start; i <= end; i++)
                yield return new LiteralNumber(new GSharpNumber(i));
    }

    public override LiteralSequence GetTail(int tailBeggining) 
    {
        if (tailBeggining > RightBound.Value.Value)
            return new LiteralArrayExpression(new List<GSharpExpression>());
        GSharpExpression leftBound = new Addition(LeftBound ,new LiteralNumber(new GSharpNumber(tailBeggining)));
        return new FiniteRangeExpression(leftBound, RightBound);
    }
}
public class InfiniteRangeExpression : LiteralSequence
{
    public InfiniteRangeExpression(GSharpExpression leftBoundExpression)
    {
        if (leftBoundExpression is not LiteralNumber)
            throw new DefaultError("Integer range sequences can only take number literals as limits", "semantic");
        
        Expressions = InfiniteRange((LiteralNumber)leftBoundExpression);
        LeftBoundExpression = leftBoundExpression;
        ExpressionType = new(GTypeNames.GSequence, GTypeNames.GNumber);
    }
    private static IEnumerable<GSharpExpression> InfiniteRange(LiteralNumber start)
    {
        double bound = start.Value.Value;
        if (!double.IsInteger(bound))
            throw new DefaultError("Sequences declaration with '...' can only take integers as limits", "Semantic");
        double i = bound;
        while (true)
        {
            yield return new LiteralNumber(new GSharpNumber(i));
            i++;
        }
    }
    public override LiteralSequence GetTail(int tailBeggining)
    {
        GSharpExpression leftBound = new Addition(LeftBoundExpression, new LiteralNumber(new GSharpNumber(tailBeggining)));
        return new InfiniteRangeExpression(leftBound);
    }
    public GSharpExpression LeftBoundExpression { get; }
}
