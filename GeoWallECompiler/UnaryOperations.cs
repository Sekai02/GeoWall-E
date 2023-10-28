namespace GeoWallECompiler;
public abstract class UnaryOperation : GSharpExpression
{
    public override GSharpObject GetValue() => Evaluate(Argument.GetValue());
    public GSharpObject Evaluate(GSharpObject arg)
    {
        return arg.GetType() == AcceptedType
            ? Operation(arg)
            : throw new SemanticError($"Operator `{OperationToken}`", EnteredType.ToString(), arg.GetType().Name);
    }
    public override GSharpTypes CheckType()
    {
        GSharpTypes argType = Argument.CheckType();
        return argType != GSharpTypes.Undetermined && argType != EnteredType
            ? throw new SemanticError($"Operator `{OperationToken}`", EnteredType.ToString(), argType.ToString())
            : ReturnedType;
    }
    public GSharpExpression Argument { get; protected set; }
    public GSharpTypes ReturnedType { get; protected set; }
    public GSharpTypes EnteredType { get; protected set; }
    public Type AcceptedType { get; protected set; }
    public string OperationToken { get; protected set; }
    public UnaryFunc Operation { get; protected set; }
    public delegate GSharpObject UnaryFunc(GSharpObject arg);
}
#region Boolean
public class Negation : UnaryOperation
{
    public Negation(GSharpExpression Arg) 
    {
        Argument = Arg;
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GObject;
        AcceptedType = typeof(GSharpObject);
        OperationToken = "not";
        GSharpNumber func(GSharpObject a) => a.ToValueOfTruth() == 0 ? new GSharpNumber(1) : new GSharpNumber(0);
        Operation = func;
    }
}
#endregion
#region Arithmetic
public class Positive : UnaryOperation
{
    public Positive(GSharpExpression Arg) 
    {
        Argument = Arg;
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "+";
        GSharpNumber func(GSharpObject a) => (GSharpNumber)a;
        Operation = func;
    }
}
public class Negative : UnaryOperation
{
    public Negative(GSharpExpression Arg) 
    {
        Argument = Arg;
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "-";
        GSharpNumber func(GSharpObject a)
        {
            var arg = a as GSharpNumber;
            return new GSharpNumber(-arg.Value);
        }
        Operation = func;
    }
}
#endregion
