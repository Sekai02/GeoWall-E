namespace GeoWallECompiler;
public abstract class UnaryOperation : GSharpExpression
{
    public override object GetValue() => Evaluate(Argument.GetValue());
    public object Evaluate(object arg)
    {
        return arg.GetType() == AcceptedType
            ? Operation((double)arg)
            : throw new SemanticError($"Operator `{OperationToken}`", EnteredType.ToString(), arg.GetType().Name);
    }
    public override GSharpTypes CheckType()
    {
        GSharpGSharpTypes argType = Argument.CheckType();
        return argType != GSharpGSharpTypes.Undetermined && argType != EnteredType
            ? throw new SemanticError($"Operator `{OperationToken}`", EnteredType.ToString(), argType.ToString())
            : ReturnedType;
    }
    public GSharpExpression Argument { get; protected set; }
    public GSharpTypes ReturnedType { get; protected set; }
    public GSharpTypes EnteredType { get; protected set; }
    public Type AcceptedType { get; protected set; }
    public string OperationToken { get; protected set; }
    public UnaryFunc Operation { get; protected set; }
    public delegate double UnaryFunc(double arg);
}
#region Boolean
public class Negation : UnaryOperation
{
    public Negation(GSharpExpression Arg) 
    {
        Argument = Arg;
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(double);
        OperationToken = "!";
        double func(double a) => a == 0 ? 1 : 0;
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
        AcceptedType = typeof(double);
        OperationToken = "+";
        double func(double a) => a;
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
        AcceptedType = typeof(double);
        OperationToken = "-";
        double func(double a) => -a;
        Operation = func;
    }
}

#endregion
