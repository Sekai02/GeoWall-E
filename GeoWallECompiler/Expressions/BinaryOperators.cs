namespace GeoWallECompiler;

/// <summary>
/// Clase abstracta de la que heredan los objetos que representan
/// a las operaciones binarias en un arbol de expresion.
/// </summary>
public abstract class BinaryOperation : GSharpExpression
{
    public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitBinaryOperation(this);
    #region Properties
    /// <summary>
    /// Expresion de G# que representa al argumeto izquierdo de la operacion binaria
    /// </summary>
    public GSharpExpression LeftArgument { get; protected set; }
    /// <summary>
    /// Expresion de G# que representa al argumeto derecho de la operacion binaria
    /// </summary>
    public GSharpExpression RightArgument { get; protected set; }
    public List<BinaryOverloadInfo> PosibleOverloads;
    /// <summary>
    /// Token de la operacion binaria
    /// </summary>
    public string OperationToken { get; protected set; }
    /// <summary>
    /// Funcion que efectuará la operacion binaria
    /// </summary>
    public BinaryFunc Operation { get; protected set; }
    /// <summary>
    /// Funcion que efectuará la operacion binaria
    /// </summary>
    public delegate GSObject BinaryFunc(GSObject left, GSObject right);
    #endregion
    public static bool IsAnAcceptedOverload(GSharpType left, GSharpType right, BinaryOverloadInfo overload)
    {
        if (left.Name != overload.LeftType.Name && left.Name != GTypeNames.Undetermined)
            return false;
        if (right.Name != overload.RightType.Name && right.Name != GTypeNames.Undetermined)
            return false;
        if(overload.LeftType.HasGenericType && overload.RightType.HasGenericType)
        {
            if (left.GenericType != right.GenericType && left.Name != GTypeNames.Undetermined && right.Name != GTypeNames.Undetermined)
                return false;
        }
        return true;
    }
    public static bool IsAnAcceptedOverload(GSObject left, GSObject right, BinaryOverloadInfo overload)
    {
        if (!left.GetType().IsAssignableTo(overload.LeftAccepted))
            return false;
        if (right.GetType().IsAssignableTo(overload.RightAccepted))
            return false;
        if (overload.LeftAccepted.ContainsGenericParameters && overload.RightAccepted.ContainsGenericParameters)
        {
            if (left.GetType() != right.GetType())
                return false;
        }
        return true;
    }
}
public class BinaryOverloadInfo
{
    public BinaryOverloadInfo(GSharpType leftType, GSharpType rightType, GSharpType returnedType, Type leftAccepted, Type rightAccepted)
    {
        LeftType = leftType;
        RightType = rightType;
        ReturnedType = returnedType;
        LeftAccepted = leftAccepted;
        RightAccepted = rightAccepted;
    }
    public static BinaryOverloadInfo GetArithmetic()
    {
        return new(new(GTypeNames.GNumber), new(GTypeNames.GNumber), new(GTypeNames.GNumber), typeof(GSNumber), typeof(GSNumber));
    }
    public static BinaryOverloadInfo GetLogic()
    {
        return new(new(GTypeNames.Undetermined), new(GTypeNames.Undetermined), new(GTypeNames.GNumber), typeof(GSObject), typeof(GSObject));
    }
    public GSharpType LeftType { get; }
    public GSharpType RightType { get; }
    public GSharpType ReturnedType { get; }
    public Type LeftAccepted { get; }
    public Type RightAccepted { get; }
}
#region Conditionals
/// <summary>
/// Clase que representa el nodo de una operacion de conjuncion en un arbol de expresion
/// </summary>
public class Conjunction : BinaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion AND
    /// </summary>
    /// <param name="leftArgument">Miembro izquierdo</param>
    /// <param name="rightArgument">Miembro derecho</param>
    public Conjunction(GSharpExpression leftArgument, GSharpExpression rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        BinaryOverloadInfo info = BinaryOverloadInfo.GetLogic();
        PosibleOverloads = new() { info };
        OperationToken = "and";
        GSObject func(GSObject a, GSObject b)
        {
            bool result = a.ToValueOfTruth() == 1 && b.ToValueOfTruth() == 1;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        Operation = func;
    }
}
/// <summary>
/// Clase que representa el nodo de una operacion de disyuncion en un arbol de expresion
/// </summary>
public class Disjunction : BinaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion OR
    /// </summary>
    /// <param name="leftArgument">Miembro izquierdo</param>
    /// <param name="rightArgument">Miembro derecho</param>
    public Disjunction(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        BinaryOverloadInfo info = BinaryOverloadInfo.GetLogic();
        PosibleOverloads = new() { info };
        OperationToken = "or";
        GSObject func(GSObject a, GSObject b)
        {
            bool result = a.ToValueOfTruth() == 1 || b.ToValueOfTruth() == 1;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        Operation = func;
    }
}
#endregion
#region Comparison
/// <summary>
/// Clase que representa el nodo de una operacion "menor que" en un arbol de expresion
/// </summary>
public class LowerThan : BinaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion <
    /// </summary>
    /// <param name="leftArgument">Miembro izquierdo</param>
    /// <param name="rightArgument">Miembro derecho</param>
    public LowerThan(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        BinaryOverloadInfo info = BinaryOverloadInfo.GetArithmetic();
        PosibleOverloads = new() { info };
        OperationToken = "<";
        GSObject func(GSObject a, GSObject b)
        {
            var result = (GSNumber)a < (GSNumber)b;
            return result? (GSNumber)1 : (GSNumber)0;
        }
        Operation = func;
    }
}
/// <summary>
/// Clase que representa el nodo de una operacion "mayor que" en un arbol de expresion
/// </summary>
public class GreaterThan : BinaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion >
    /// </summary>
    /// <param name="leftArgument">Miembro izquierdo</param>
    /// <param name="rightArgument">Miembro derecho</param>
    public GreaterThan(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        BinaryOverloadInfo info = BinaryOverloadInfo.GetArithmetic();
        PosibleOverloads = new() { info };
        OperationToken = ">";
        GSObject func(GSObject a, GSObject b)
        {
            var result = (GSNumber)a > (GSNumber)b;
            return result? (GSNumber)1 : (GSNumber)0;
        }
        Operation = func;
    }
}
/// <summary>
/// Clase que representa el nodo de una operacion "menor igual que" en un arbol de expresion
/// </summary>
public class LowerEqualThan : BinaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion <=
    /// </summary>
    /// <param name="leftArgument">Miembro izquierdo</param>
    /// <param name="rightArgument">Miembro derecho</param>
    public LowerEqualThan(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        BinaryOverloadInfo info = BinaryOverloadInfo.GetArithmetic();
        PosibleOverloads = new() { info };
        OperationToken = "<=";
        GSObject func(GSObject a, GSObject b)
        {
            var result = (GSNumber)a <= (GSNumber)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        Operation = func;
    }
}
/// <summary>
/// Clase que representa el nodo de una operacion "mayor igual que" en un arbol de expresion
/// </summary>
public class GreaterEqualThan : BinaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion >=
    /// </summary>
    /// <param name="leftArgument">Miembro izquierdo</param>
    /// <param name="rightArgument">Miembro derecho</param>
    public GreaterEqualThan(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        BinaryOverloadInfo info = BinaryOverloadInfo.GetArithmetic();
        PosibleOverloads = new() { info };
        OperationToken = ">=";
        GSObject func(GSObject a, GSObject b)
        {
            var result = (GSNumber)a >= (GSNumber)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        Operation = func;
    }
}
/// <summary>
/// Clase que representa el nodo de una operacion de igualdad en un arbol de expresion
/// </summary>
public class Equal : BinaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion ==
    /// </summary>
    /// <param name="leftArgument">Miembro izquierdo</param>
    /// <param name="rightArgument">Miembro derecho</param>
    public Equal(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        BinaryOverloadInfo info = BinaryOverloadInfo.GetArithmetic();
        PosibleOverloads = new() { info };
        OperationToken = "==";
        GSObject func(GSObject a, GSObject b)
        {
            var result = (GSNumber)a == (GSNumber)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        Operation = func;
    }
}
/// <summary>
/// Clase que representa el nodo de una operacion de desigualdad en un arbol de expresion
/// </summary>
public class UnEqual : BinaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion !=
    /// </summary>
    /// <param name="leftArgument">Miembro izquierdo</param>
    /// <param name="rightArgument">Miembro derecho</param>
    public UnEqual(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        BinaryOverloadInfo info = BinaryOverloadInfo.GetArithmetic();
        PosibleOverloads = new() { info };
        OperationToken = "!=";
        GSObject func(GSObject a, GSObject b)
        {
            var result = (GSNumber)a != (GSNumber)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        Operation = func;
    }
}
#endregion
#region Arithmetic Basic Operations
/// <summary>
/// Clase que representa el nodo de una operacion de adición en un arbol de expresion
/// </summary>
public class Addition : BinaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion +
    /// </summary>
    /// <param name="leftArgument">Sumando izquierdo</param>
    /// <param name="rightArgument">Sumando derecho</param>
    public Addition(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        BinaryOverloadInfo info = BinaryOverloadInfo.GetArithmetic();
        PosibleOverloads = new() { info };
        OperationToken = "+";
        GSObject func(GSObject a, GSObject b) => (GSNumber)a + (GSNumber)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa el nodo de una operacion de sustracción en un arbol de expresion
/// </summary>
public class Subtraction : BinaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion -
    /// </summary>
    /// <param name="leftArgument">Expresion minuendo</param>
    /// <param name="rightArgument">Expresion sustraendo</param>
    public Subtraction(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        BinaryOverloadInfo info = BinaryOverloadInfo.GetArithmetic();
        PosibleOverloads = new() { info };
        OperationToken = "-";
        GSObject func(GSObject a, GSObject b) => (GSNumber)a - (GSNumber)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa el nodo de una operacion de multiplicación en un arbol de expresion
/// </summary>
public class Multiplication : BinaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion *
    /// </summary>
    /// <param name="leftArgument">Producto izquierdo</param>
    /// <param name="rightArgument">Producto derecho</param>
    public Multiplication(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        BinaryOverloadInfo info = BinaryOverloadInfo.GetArithmetic();
        PosibleOverloads = new() { info };
        OperationToken = "*";
        GSObject func(GSObject a, GSObject b) => (GSNumber)a * (GSNumber)b; 
        Operation = func;
    }
}
/// <summary>
/// Clase que representa el nodo de una operacion de division en un arbol de expresion
/// </summary>
public class Division : BinaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion /
    /// </summary>
    /// <param name="leftArgument">Expresion divisor</param>
    /// <param name="rightArgument">Expresion dividendo</param>
    public Division(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        BinaryOverloadInfo info = BinaryOverloadInfo.GetArithmetic();
        PosibleOverloads = new() { info };
        OperationToken = "/";
        GSObject func(GSObject a, GSObject b) => 
            b.ToValueOfTruth() == 0 ? throw new DefaultError("Atempted to divide by 0", "arithmetic") : (GSNumber)a / (GSNumber)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa el nodo de una operacion de modulo o resto en un arbol de expresion
/// </summary>
public class Module : BinaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion %
    /// </summary>
    /// <param name="leftArgument">Expresion divisor</param>
    /// <param name="rightArgument">Expresion dividendo</param>
    public Module(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        BinaryOverloadInfo info = BinaryOverloadInfo.GetArithmetic();
        PosibleOverloads = new() { info };
        OperationToken = "%";
        GSObject func(GSObject a, GSObject b) =>
            b.ToValueOfTruth() == 0 ? throw new DefaultError("Atempted to divide by 0", "arithmetic") : (GSNumber)a % (GSNumber)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa el nodo de una operacion de potencia en un arbol de expresion
/// </summary>
public class Power : BinaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion ^
    /// </summary>
    /// <param name="leftArgument">Expresion base</param>
    /// <param name="rightArgument">Expresion exponente</param>
    public Power(GSharpExpression leftArgument, GSharpExpression rightArgument) 
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        BinaryOverloadInfo info = BinaryOverloadInfo.GetArithmetic();
        PosibleOverloads = new() { info };
        OperationToken = "^";
        GSObject func(GSObject a, GSObject b)
        {
            GSNumber left = (GSNumber)a;
            GSNumber right = (GSNumber)b;
            return (GSNumber)Math.Pow(left, right);
        }
        Operation = func;
    }
}
#endregion
