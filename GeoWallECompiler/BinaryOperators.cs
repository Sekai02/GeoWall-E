namespace GeoWallECompiler;
public abstract class BinaryOperation : GSharpExpression
{
    #region Methods
    public override object GetValue() => Evaluate(LeftArgument.GetValue(), RightArgument.GetValue());
    public override GSharpTypes CheckType()
    {
        if (EnteredType == GSharpTypes.Undetermined)
            return ReturnedType;
        var leftType = LeftArgument.CheckType();
        var rightType = RightArgument.CheckType();
        if (leftType != EnteredType && leftType != GSharpTypes.Undetermined)
            throw new SemanticError($"Operator `{OperationToken}`", EnteredType.ToString(), leftType.ToString());
        if (rightType != EnteredType && rightType != GSharpTypes.Undetermined)
            throw new SemanticError($"Operator `{OperationToken}`", EnteredType.ToString(), rightType.ToString());
        return ReturnedType;
    }
    public object Evaluate(object left, object right)
    {
        if (left.GetType() == AcceptedType && right.GetType() == AcceptedType || AcceptedType == typeof(object))
            return Operation((double)left, (double)right);
        var conflictiveType = left.GetType() != AcceptedType ? left.GetType().Name : right.GetType().Name;
        throw new SemanticError($"Operator `{OperationToken}`", ReturnedType.ToString(), conflictiveType);
    }
    #endregion
    #region Properties
    public GSharpExpression LeftArgument { get; protected set; }
    public GSharpExpression RightArgument { get; protected set; }
    public GSharpTypes ReturnedType { get; protected set; }
    public GSharpTypes EnteredType { get; protected set; }
    public Type AcceptedType { get; protected set; }
    public string OperationToken { get; protected set; }
    public BinaryFunc Operation { get; protected set; }
    public delegate double BinaryFunc(double left, double right);
    #endregion
}
#region Conditionals
public class Conjunction : BinaryOperation
{
    public Conjunction(GSharpExpression leftArgument, GSharpExpression rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = GSharpTypes.GObject;
        EnteredType = GSharpTypes.GObject;
        AcceptedType = typeof(object); //aqui se va a cambiar por Gobject
        OperationToken = "and";
        double func(double a, double b)
        {
            bool result = a == 1 && b == 1;
            return result ? 1 : 0;
        }
        Operation = func;
    }
}
public class Disjunction : BinaryOperation
{
    public Disjunction(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = GSharpTypes.GObject;
        EnteredType = GSharpTypes.GObject;
        AcceptedType = typeof(object); //aqui se va a cambiar por Gobject
        OperationToken = "or";
        double func(double a, double b)
        {
            bool result = a == 1 || b == 1;
            return result ? 1 : 0;
        }
        Operation = func;
    }
}
#endregion
#region Comparison
public class LowerThan : BinaryOperation
{
    public LowerThan(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType =GSharpTypes.GNumber;
        EnteredType =GSharpTypes.GNumber;
        AcceptedType = typeof(double);
        OperationToken = "<";
        double func(double a, double b) 
        {
            bool result = a < b;
            return result ? 1 : 0;
        }
        Operation = func;
    }
}
public class GreaterThan : BinaryOperation
{
    public GreaterThan(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType =GSharpTypes.GNumber;
        EnteredType =GSharpTypes.GNumber;
        AcceptedType = typeof(double);
        OperationToken = ">";
        double func(double a, double b)
        {
            bool result = a > b;
            return result ? 1 : 0;
        }
        Operation = func;
    }
}
public class LowerEqualThan : BinaryOperation
{
    public LowerEqualThan(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType =GSharpTypes.GNumber;
        EnteredType =GSharpTypes.GNumber;
        AcceptedType = typeof(double);
        OperationToken = "<=";
        double func(double a, double b)
        {
            bool result = a <= b;
            return result ? 1 : 0;
        }
        Operation = func;
    }
}
public class GreaterEqualThan : BinaryOperation
{
    public GreaterEqualThan(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType =GSharpTypes.GNumber;
        EnteredType =GSharpTypes.GNumber;
        AcceptedType = typeof(double);
        OperationToken = ">=";
        double func(double a, double b)
        {
            bool result = a >= b;
            return result ? 1 : 0;
        }
        Operation = func;
    }
}
public class Equal : BinaryOperation
{
    public Equal(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType =GSharpTypes.GNumber;
        EnteredType =GSharpTypes.GNumber;
        AcceptedType = typeof(object);
        OperationToken = "==";
        double func(double a, double b)
        {
            bool result = a == b;
            return result ? 1 : 0;
        }
        Operation = func;
    }
}
public class UnEqual : BinaryOperation
{
    public UnEqual(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType =GSharpTypes.GNumber;
        EnteredType =GSharpTypes.GNumber;
        AcceptedType = typeof(object);
        OperationToken = "!=";
        double func(double a, double b)
        {
            bool result = a != b;
            return result ? 1 : 0;
        }
        Operation = func;
    }
}
#endregion
#region Arithmetic Basic Operations
public class Addition : BinaryOperation
{
    public Addition(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType =GSharpTypes.GNumber;
        EnteredType =GSharpTypes.GNumber;
        AcceptedType = typeof(double);
        OperationToken = "+";
        double func(double a, double b) => a + b;
        Operation = func;
    }
}
public class Subtraction : BinaryOperation
{
    public Subtraction(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType =GSharpTypes.GNumber;
        EnteredType =GSharpTypes.GNumber;
        AcceptedType = typeof(double);
        OperationToken = "-";
        double func(double a, double b) => a - b;
        Operation = func;
    }
}
public class Multiplication : BinaryOperation
{
    public Multiplication(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType =GSharpTypes.GNumber;
        EnteredType =GSharpTypes.GNumber;
        AcceptedType = typeof(double);
        OperationToken = "*";
        double func(double a, double b) => a * b;
        Operation = func;
    }
}
public class Division : BinaryOperation
{
    public Division(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType =GSharpTypes.GNumber;
        EnteredType =GSharpTypes.GNumber;
        AcceptedType = typeof(double);
        OperationToken = "/";
        double func(double a, double b)
        {
            return b == 0 ? throw new DefaultError("Atempted to divide by 0", "arithmetic") : (a / b);
        }
        Operation = func;
    }
}
public class Module : BinaryOperation
{
    public Module(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType =GSharpTypes.GNumber;
        EnteredType =GSharpTypes.GNumber;
        AcceptedType = typeof(double);
        OperationToken = "%";
        double func(double a, double b)
        {
            return b == 0 ? throw new DefaultError("Atempted to divide by 0", "arithmetic") : (a % b);
        }
        Operation = func;
    }
}
public class Power : BinaryOperation
{
    public Power(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType =GSharpTypes.GNumber;
        EnteredType =GSharpTypes.GNumber;
        AcceptedType = typeof(double);
        OperationToken = "^";
        double func(double a, double b) => Math.Pow(a, b);
        Operation = func;
    }
}
#endregion
