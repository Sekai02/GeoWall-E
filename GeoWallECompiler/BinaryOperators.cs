namespace GeoWallECompiler;
public abstract class BinaryOperation : GSharpExpression
{
    #region Methods
    public override GSharpObject GetValue() => Evaluate(LeftArgument.GetValue(), RightArgument.GetValue());
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
    public GSharpObject Evaluate(GSharpObject left, GSharpObject right)
    {
        if (left.GetType() == AcceptedType && right.GetType() == AcceptedType)
            return Operation(left, right);
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
    public delegate GSharpObject BinaryFunc(GSharpObject left, GSharpObject right);
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
        AcceptedType = typeof(GSharpObject);
        OperationToken = "and";
        GSharpObject func(GSharpObject a, GSharpObject b)
        {
            bool result = a.ToValueOfTruth() == 1 && b.ToValueOfTruth() == 1;
            return result ? new GSharpNumber(1) : new GSharpNumber(0);
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
        AcceptedType = typeof(GSharpObject);
        OperationToken = "or";
        GSharpObject func(GSharpObject a, GSharpObject b)
        {
            bool result = a.ToValueOfTruth() == 1 || b.ToValueOfTruth() == 1;
            return result ? new GSharpNumber(1) : new GSharpNumber(0);
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
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "<";
        GSharpObject func(GSharpObject a, GSharpObject b)
        {
            var result = (GSharpNumber)a < (GSharpNumber)b;
            return result.ToValueOfTruth() == 1 ? new GSharpNumber(1) : new GSharpNumber(0);
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
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = ">";
        GSharpObject func(GSharpObject a, GSharpObject b)
        {
            var result = (GSharpNumber)a > (GSharpNumber)b;
            return result.ToValueOfTruth() == 1 ? new GSharpNumber(1) : new GSharpNumber(0);
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
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "<=";
        GSharpObject func(GSharpObject a, GSharpObject b)
        {
            var result = (GSharpNumber)a <= (GSharpNumber)b;
            return result.ToValueOfTruth() == 1 ? new GSharpNumber(1) : new GSharpNumber(0);
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
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = ">=";
        GSharpObject func(GSharpObject a, GSharpObject b)
        {
            var result = (GSharpNumber)a >= (GSharpNumber)b;
            return result.ToValueOfTruth() == 1 ? new GSharpNumber(1) : new GSharpNumber(0);
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
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "==";
        GSharpObject func(GSharpObject a, GSharpObject b)
        {
            var result = (GSharpNumber)a == (GSharpNumber)b;
            return result.ToValueOfTruth() == 1 ? new GSharpNumber(1) : new GSharpNumber(0);
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
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "!=";
        GSharpObject func(GSharpObject a, GSharpObject b)
        {
            var result = (GSharpNumber)a != (GSharpNumber)b;
            return result.ToValueOfTruth() == 1 ? new GSharpNumber(1) : new GSharpNumber(0);
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
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "+";
        GSharpObject func(GSharpObject a, GSharpObject b) => (GSharpNumber)a + (GSharpNumber)b;
        Operation = func;
    }
}
public class Subtraction : BinaryOperation
{
    public Subtraction(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "-";
        GSharpObject func(GSharpObject a, GSharpObject b) => (GSharpNumber)a - (GSharpNumber)b;
        Operation = func;
    }
}
public class Multiplication : BinaryOperation
{
    public Multiplication(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "*";
        GSharpObject func(GSharpObject a, GSharpObject b) => (GSharpNumber)a * (GSharpNumber)b; 
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
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "/";
        GSharpObject func(GSharpObject a, GSharpObject b) => 
            b.ToValueOfTruth() == 0 ? throw new DefaultError("Atempted to divide by 0", "arithmetic") : (GSharpNumber)a / (GSharpNumber)b;
        Operation = func;
    }
}
public class Module : BinaryOperation
{
    public Module(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "%";
        GSharpObject func(GSharpObject a, GSharpObject b) =>
            b.ToValueOfTruth() == 0 ? throw new DefaultError("Atempted to divide by 0", "arithmetic") : (GSharpNumber)a % (GSharpNumber)b;
        Operation = func;
    }
}
public class Power : BinaryOperation
{
    public Power(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "^";
        GSharpObject func(GSharpObject a, GSharpObject b)
        {
            GSharpNumber left = (GSharpNumber)a;
            GSharpNumber right = (GSharpNumber)b;
            return new GSharpNumber(Math.Pow(left.Value,right.Value));
        }
        Operation = func;
    }
}
#endregion
