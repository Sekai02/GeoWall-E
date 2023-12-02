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
    /// <summary>
    /// Tipo de retorno de la operacion binaria en forma de enum
    /// </summary>
    public GSharpType ReturnedType { get; protected set; }
    /// <summary>
    /// Tipo de entrada de la operacion binaria en forma de enum
    /// </summary>
    public GSharpType EnteredType { get; protected set; }
    /// <summary>
    /// Tipo aceptado de la operacion binaria en forma de Type
    /// </summary>
    public Type AcceptedType { get; protected set; }
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GObject);
        AcceptedType = typeof(GSObject);
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GObject);
        AcceptedType = typeof(GSObject);
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GNumber);
        AcceptedType = typeof(GSNumber);
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GNumber);
        AcceptedType = typeof(GSNumber);
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GNumber);
        AcceptedType = typeof(GSNumber);
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GNumber);
        AcceptedType = typeof(GSNumber);
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GObject);
        AcceptedType = typeof(GSNumber);
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GObject);
        AcceptedType = typeof(GSNumber);
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GNumber);
        AcceptedType = typeof(GSNumber);
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GNumber);
        AcceptedType = typeof(GSNumber);
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GNumber);
        AcceptedType = typeof(GSNumber);
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GNumber);
        AcceptedType = typeof(GSNumber);
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GNumber);
        AcceptedType = typeof(GSNumber);
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
        ReturnedType = new(GTypeNames.GNumber);
        EnteredType = new(GTypeNames.GNumber);
        AcceptedType = typeof(GSNumber);
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
