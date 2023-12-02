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
    /// Tipo de retorno de la operacion unaria en forma de enum
    /// </summary>
    public GSharpType ReturnedType { get; protected set; }
    /// <summary>
    /// Tipo de entrada de la operacion unaria en forma de enum
    /// </summary>
    public GSharpType EnteredType { get; protected set; }
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
    public delegate GSObject UnaryFunc(GSObject arg);
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GObject);
        AcceptedType = typeof(GSObject);
        OperationToken = "not";
        GSNumber func(GSObject a) => a.ToValueOfTruth() == 0 ? (GSNumber)1 : (GSNumber)0;
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GNumber);
        AcceptedType = typeof(GSNumber);
        OperationToken = "+";
        GSNumber func(GSObject a) => (GSNumber)a;
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GNumber);
        AcceptedType = typeof(GSNumber);
        OperationToken = "-";
        GSNumber func(GSObject a) => -(GSNumber)a;
        Operation = func;
    }
}
#endregion
