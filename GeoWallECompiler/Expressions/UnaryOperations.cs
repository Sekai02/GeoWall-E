namespace GeoWallECompiler;

/// <summary>
/// Clase abstracta de la que heredan los objetos que representan
/// a las operaciones unarias en un arbol de expresion.
/// </summary>
public abstract class UnaryOperation : GSharpExpression
{
    public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitUnaryOperation(this);
    /// <summary>
    /// Argumento de la operacion unaria
    /// </summary>
    public GSharpExpression Argument { get; protected set; }
    /// <summary>
    /// Token de la operacion
    /// </summary>
    public string OperationToken { get; protected set; }
    public List<UnaryOverloadInfo> PosibleOverloads { get; protected set; }
    public static bool IsAnAcceptedOverload(GSharpType arg, UnaryOverloadInfo overload)
    {
        return arg.Name == overload.ArgumentType.Name || 
            arg.Name == GTypeNames.Undetermined || 
            overload.ArgumentType.Name != GTypeNames.Undetermined;
    }
    public static bool IsAnAcceptedOverload(GSObject? arg, UnaryOverloadInfo overload)
    {
        if (arg is not null)
            if (!arg.GetType().IsAssignableTo(overload.ArgumentAccepted))
                return false;
        return true;
    }
}
public class UnaryOverloadInfo
{
    public UnaryOverloadInfo(GSharpType argType, GSharpType returnedType, UnaryFunc operation)
    {
        ArgumentType = argType;
        ReturnedType = returnedType;
        ArgumentAccepted = GSharpType.ConvertToType(argType.Name);
        Operation = operation;
    }
    public UnaryFunc Operation { get; protected set; }
    public delegate GSObject? UnaryFunc(GSObject? arg);
    public static UnaryOverloadInfo GetArithmetic(UnaryFunc operation)
    {
        return new(new(GTypeNames.GNumber), new(GTypeNames.GNumber), operation);
    }
    public static UnaryOverloadInfo GetLogic(UnaryFunc operation)
    {
        return new(new(GTypeNames.GObject), new(GTypeNames.GNumber), operation);
    }
    public GSharpType ArgumentType { get; }
    public GSharpType ReturnedType { get; }
    public Type ArgumentAccepted { get; }
}
#region Boolean

/// <summary>
/// Clase que representa el nodo de una operacion de negacion en un arbol de expresion
/// </summary>
public class Negation : UnaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion NOT
    /// </summary>
    /// <param name="Arg">Argumento de la operacion</param>
    public Negation(GSharpExpression Arg) 
    {
        Argument = Arg;
        GSNumber? func(GSObject? a) => GSObject.ToValueOfTruth(a) == 0 ? (GSNumber)1 : (GSNumber)0;
        UnaryOverloadInfo info = UnaryOverloadInfo.GetLogic(func);
        OperationToken = "not";
        PosibleOverloads = new() { info };
    }
}
#endregion
#region Arithmetic

/// <summary>
/// Clase que representa el nodo de una operacion positiva en un arbol de expresion
/// </summary>
public class Positive : UnaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion +
    /// </summary>
    /// <param name="Arg">Argumento de la operacion. Debe ser de tipo GSharpNumber</param>
    public Positive(GSharpExpression Arg) 
    {
        Argument = Arg;
        GSNumber? func(GSObject? a) => (GSNumber?)a;
        UnaryOverloadInfo info = UnaryOverloadInfo.GetArithmetic(func);
        OperationToken = "+";
        PosibleOverloads = new() { info };
    }
}

/// <summary>
/// Clase que representa el nodo de una operacion de negativa en un arbol de expresion
/// </summary>
public class Negative : UnaryOperation
{
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion -
    /// </summary>
    /// <param name="Arg">Argumento de la operacion. Debe ser de tipo GSharpNumber</param>
    public Negative(GSharpExpression Arg) 
    {
        Argument = Arg;
        GSNumber? func(GSObject? a) => -(GSNumber?)a;
        UnaryOverloadInfo info = UnaryOverloadInfo.GetArithmetic(func);
        OperationToken = "-";
        PosibleOverloads = new() { info };
    }
}
#endregion
