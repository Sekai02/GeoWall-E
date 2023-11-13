﻿namespace GeoWallECompiler;

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
    public GSharpTypes ReturnedType { get; protected set; }
    /// <summary>
    /// Tipo de entrada de la operacion binaria en forma de enum
    /// </summary>
    public GSharpTypes EnteredType { get; protected set; }
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
    public delegate GSharpObject BinaryFunc(GSharpObject left, GSharpObject right);
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
    /// <summary>
    /// Crea una instancia de una clase que representa a la operacion +
    /// </summary>
    /// <param name="leftArgument">Sumando izquierdo</param>
    /// <param name="rightArgument">Sumando derecho</param>
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
/// <summary>
/// Clase que representa el nodo de una operacion de adición en un arbol de expresion
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
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "-";
        GSharpObject func(GSharpObject a, GSharpObject b) => (GSharpNumber)a - (GSharpNumber)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa el nodo de una operacion de sustracción en un arbol de expresion
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
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "*";
        GSharpObject func(GSharpObject a, GSharpObject b) => (GSharpNumber)a * (GSharpNumber)b; 
        Operation = func;
    }
}
/// <summary>
/// Clase que representa el nodo de una operacion de multiplicación en un arbol de expresion
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
        ReturnedType =GSharpTypes.GNumber;
        EnteredType =GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "/";
        GSharpObject func(GSharpObject a, GSharpObject b) => 
            b.ToValueOfTruth() == 0 ? throw new DefaultError("Atempted to divide by 0", "arithmetic") : (GSharpNumber)a / (GSharpNumber)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa el nodo de una operacion de division en un arbol de expresion
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
        ReturnedType = GSharpTypes.GNumber;
        EnteredType = GSharpTypes.GNumber;
        AcceptedType = typeof(GSharpNumber);
        OperationToken = "%";
        GSharpObject func(GSharpObject a, GSharpObject b) =>
            b.ToValueOfTruth() == 0 ? throw new DefaultError("Atempted to divide by 0", "arithmetic") : (GSharpNumber)a % (GSharpNumber)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa el nodo de una operacion de modulo o resto en un arbol de expresion
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
/// <summary>
/// Clase que representa el nodo de una operacion de potencia en un arbol de expresion
/// </summary>
#endregion
