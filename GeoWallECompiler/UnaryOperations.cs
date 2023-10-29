namespace GeoWallECompiler;

/// <summary>
/// Clase abstracta de la que heredan los objetos que representan
/// a las operaciones unarias en un arbol de expresion.
/// </summary>
public abstract class UnaryOperation : GSharpExpression
{
    public override GSharpObject GetValue() => Evaluate(Argument.GetValue());
    /// <summary>
    /// Evalua la expresion unaria, luego de chequear que el tipo de entrada sea correcto
    /// </summary>
    /// <param name="arg">Valor argumento</param>
    /// <returns>Resultado de evaluar la operacion unaria</returns>
    /// <exception cref="SemanticError">Se lanza cuando el tipos de entrada no es el mismo que el tipo de entrada de la operacion</exception>
    private GSharpObject Evaluate(GSharpObject arg)
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
    /// <summary>
    /// Argumento de la operacion unaria
    /// </summary>
    public GSharpExpression Argument { get; protected set; }
    /// <summary>
    /// Tipo de retorno de la operacion unaria en forma de enum
    /// </summary>
    public GSharpTypes ReturnedType { get; protected set; }
    /// <summary>
    /// Tipo de entrada de la operacion unaria en forma de enum
    /// </summary>
    public GSharpTypes EnteredType { get; protected set; }
    /// <summary>
    /// Tipo de entrada de la operacion unaria en forma de Type
    /// </summary>
    public Type AcceptedType { get; protected set; }
    /// <summary>
    /// Token de la operacion
    /// </summary>
    public string OperationToken { get; protected set; }
    /// <summary>
    /// Funcion que efectuara la operacion unaria
    /// </summary>
    public UnaryFunc Operation { get; protected set; }
    /// <summary>
    /// Funcion unaria que efectuara la operacion
    /// </summary>
    /// <param name="arg">Valor del argumento de la operacion</param>
    /// <returns></returns>
    public delegate GSharpObject UnaryFunc(GSharpObject arg);
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
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "+";
        GSharpNumber func(GSharpObject a) => (GSharpNumber)a;
        Operation = func;
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
