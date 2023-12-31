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
    public List<BinaryOverloadInfo> PosibleOverloads { get; protected set; }
    /// <summary>
    /// Token de la operacion binaria
    /// </summary>
    public string OperationToken { get; protected set; }
    #endregion
    public static bool IsAnAcceptedOverload(GSharpType left, GSharpType right, BinaryOverloadInfo overload)
    {
        if (left.Name != overload.LeftType.Name && left.Name != GTypeNames.Undetermined && overload.LeftType.Name != GTypeNames.GObject)
            return false;
        if (right.Name != overload.RightType.Name && right.Name != GTypeNames.Undetermined && overload.RightType.Name != GTypeNames.GObject)
            return false;
        if(overload.LeftType.HasGenericType && overload.RightType.HasGenericType)
        {
            if (left.Name == GTypeNames.Undetermined || right.Name == GTypeNames.Undetermined)
                return true;
            if (left.GenericType != right.GenericType)
                return false;
        }
        return true;
    }
    public static bool IsAnAcceptedOverload(GSObject? left, GSObject? right, BinaryOverloadInfo overload)
    {
        if(left is not null)
            if (!left.GetType().IsAssignableTo(overload.LeftAccepted))
                return false;
        if(right is not null)
            if (!right.GetType().IsAssignableTo(overload.RightAccepted))
                return false;
        if (overload.LeftAccepted.ContainsGenericParameters && overload.RightAccepted.ContainsGenericParameters)
        {
            if(left is not null && right is not null)
                if (left.GetType() != right.GetType())
                    return false;
        }
        return true;
    }
}
public class BinaryOverloadInfo
{
    public BinaryOverloadInfo(GSharpType leftType,
                              GSharpType rightType,
                              GSharpType returnedType,
                              BinaryFunc operation,
                              TypeGetter? getType = null)
    {
        LeftType = leftType;
        RightType = rightType;
        ReturnedType = returnedType;
        LeftAccepted = GSharpType.ConvertToType(leftType.Name);
        RightAccepted = GSharpType.ConvertToType(rightType.Name);
        Operation = operation;
        if(getType is null)
        {
            GSharpType func(GSharpType a, GSharpType b) => returnedType;
            GetType = func;
        }
        else
            GetType = getType;
    }
    public BinaryOverloadInfo(GSharpType parameterType, GSharpType returnedType, BinaryFunc operation, TypeGetter? getType = null)
    {
        LeftType = RightType = parameterType;
        ReturnedType = returnedType;
        LeftAccepted = RightAccepted = GSharpType.ConvertToType(parameterType.Name);
        Operation = operation;
        if (getType is null)
        {
            GSharpType func(GSharpType a, GSharpType b) => returnedType;
            GetType = func;
        }
        else
            GetType = getType;
    }
    public BinaryFunc Operation { get; protected set; }
    public TypeGetter GetType { get; protected set; }
    public delegate GSObject? BinaryFunc(GSObject? left, GSObject? right);
    public delegate GSharpType TypeGetter(GSharpType left, GSharpType right);
    public static BinaryOverloadInfo GetArithmetic(BinaryFunc operation)
    {
        return new(new(GTypeNames.GNumber), new(GTypeNames.GNumber), operation);
    }
    public static BinaryOverloadInfo GetLogic(BinaryFunc operation)
    {
        return new(new(GTypeNames.Undetermined), new(GTypeNames.GNumber), operation);
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
        GSObject? func(GSObject? a, GSObject? b)
        {
            bool result = GSObject.ToValueOfTruth(a) == 1 && GSObject.ToValueOfTruth(b) == 1;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo info = BinaryOverloadInfo.GetLogic(func);
        PosibleOverloads = new() { info };
        OperationToken = "and";
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
        GSObject? func(GSObject? a, GSObject? b)
        {
            bool result = GSObject.ToValueOfTruth(a) == 1 || GSObject.ToValueOfTruth(b) == 1;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo info = BinaryOverloadInfo.GetLogic(func);
        PosibleOverloads = new() { info };
        OperationToken = "or";
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
        GSObject? compareNumbers(GSObject? a, GSObject? b)
        {
            var result = (GSNumber?)a < (GSNumber?)b;
            return result? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo compareNumbersInfo = BinaryOverloadInfo.GetArithmetic(compareNumbers);
        GSObject? compareMeasures(GSObject? a, GSObject? b)
        {
            var result = (Measure?)a < (Measure?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo compareMeasuresInfo = new(new(GTypeNames.Measure), new(GTypeNames.GNumber), compareMeasures);
        GSObject? measureLowerNatural(GSObject? a, GSObject? b) 
        {
            var result = (Measure?)a < (GSNumber?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo measureLowerNaturalInfo = new(new(GTypeNames.Measure), new(GTypeNames.GNumber), new(GTypeNames.GNumber), measureLowerNatural);

        GSObject? naturalLowerMeasure(GSObject? a, GSObject? b)
        {
            var result = (GSNumber?)a < (Measure?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo naturalLowerMeasureInfo = new(new(GTypeNames.GNumber), new(GTypeNames.Measure), new(GTypeNames.GNumber), naturalLowerMeasure);
        PosibleOverloads = new() { compareNumbersInfo, compareMeasuresInfo, measureLowerNaturalInfo, naturalLowerMeasureInfo};
        OperationToken = "<";
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
        GSObject? compareNumbers(GSObject? a, GSObject? b)
        {
            var result = (GSNumber?)a > (GSNumber?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo compareNumbersInfo = BinaryOverloadInfo.GetArithmetic(compareNumbers);
        GSObject? compareMeasures(GSObject? a, GSObject? b)
        {
            var result = (Measure?)a > (Measure?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo compareMeasuresInfo = new(new(GTypeNames.Measure), new(GTypeNames.GNumber), compareMeasures);
        GSObject? measureGreaterNatural(GSObject? a, GSObject? b)
        {
            var result = (Measure?)a > (GSNumber?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo measureGreaterNaturalInfo = new(new(GTypeNames.Measure), new(GTypeNames.GNumber), new(GTypeNames.GNumber), measureGreaterNatural);

        GSObject? naturalGreaterMeasure(GSObject? a, GSObject? b)
        {
            var result = (GSNumber?)a > (Measure?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo naturalGreaterMeasureInfo = new(new(GTypeNames.GNumber), new(GTypeNames.Measure), new(GTypeNames.GNumber), naturalGreaterMeasure);
        PosibleOverloads = new() { compareNumbersInfo, compareMeasuresInfo, measureGreaterNaturalInfo, naturalGreaterMeasureInfo};
        OperationToken = ">";
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
        GSObject? compareNumbers(GSObject? a, GSObject? b)
        {
            var result = (GSNumber?)a <= (GSNumber?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo compareNumbersInfo = BinaryOverloadInfo.GetArithmetic(compareNumbers);
        GSObject? compareMeasures(GSObject? a, GSObject? b)
        {
            var result = (Measure?)a <= (Measure?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo compareMeasuresInfo = new(new(GTypeNames.Measure), new(GTypeNames.GNumber), compareMeasures);
        GSObject? measureLowerEqualNatural(GSObject? a, GSObject? b)
        {
            var result = (Measure?)a <= (GSNumber?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo measureLowerEqualNaturalInfo = new(new(GTypeNames.Measure), new(GTypeNames.GNumber), new(GTypeNames.GNumber), measureLowerEqualNatural);

        GSObject? naturalLowerEqualMeasure(GSObject? a, GSObject? b)
        {
            var result = (GSNumber?)a <= (Measure?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo naturalLowerEqualMeasureInfo = new(new(GTypeNames.GNumber), new(GTypeNames.Measure), new(GTypeNames.GNumber), naturalLowerEqualMeasure);
        PosibleOverloads = new() { compareNumbersInfo, compareMeasuresInfo, measureLowerEqualNaturalInfo, naturalLowerEqualMeasureInfo };
        OperationToken = "<=";
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
        GSObject? compareNumbers(GSObject? a, GSObject? b)
        {
            var result = (GSNumber?)a >= (GSNumber?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo compareNumbersInfo = BinaryOverloadInfo.GetArithmetic(compareNumbers);
        GSObject? compareMeasures(GSObject? a, GSObject? b)
        {
            var result = (Measure?)a >= (Measure?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo compareMeasuresInfo = new(new(GTypeNames.Measure), new(GTypeNames.GNumber), compareMeasures);
        GSObject? measureGreaterEqualNatural(GSObject? a, GSObject? b)
        {
            var result = (Measure?)a > (GSNumber?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo measureGreaterEqualNaturalInfo = new(new(GTypeNames.Measure), new(GTypeNames.GNumber), new(GTypeNames.GNumber), measureGreaterEqualNatural);

        GSObject? naturalGreaterEqualMeasure(GSObject? a, GSObject? b)
        {
            var result = (GSNumber?)a > (Measure?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo naturalGreaterEqualMeasureInfo = new(new(GTypeNames.GNumber), new(GTypeNames.Measure), new(GTypeNames.GNumber), naturalGreaterEqualMeasure);
        PosibleOverloads = new() { compareNumbersInfo, compareMeasuresInfo, measureGreaterEqualNaturalInfo, naturalGreaterEqualMeasureInfo };
        OperationToken = ">=";
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
        GSObject? compareNumbers(GSObject? a, GSObject? b)
        {
            var result = (GSNumber?)a == (GSNumber?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo compareNumbersInfo = BinaryOverloadInfo.GetArithmetic(compareNumbers);
        GSObject? compareMeasures(GSObject? a, GSObject? b)
        {
            var result = (Measure?)a == (Measure?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo compareMeasuresInfo = new(new(GTypeNames.Measure), new(GTypeNames.GNumber), compareMeasures);
        GSObject? measureEqualNatural(GSObject? a, GSObject? b)
        {
            var result = (Measure?)a == (GSNumber?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo measureEqualNaturalInfo = new(new(GTypeNames.Measure), new(GTypeNames.GNumber), new(GTypeNames.GNumber), measureEqualNatural);

        GSObject? naturalEqualMeasure(GSObject? a, GSObject? b)
        {
            var result = (GSNumber?)a == (Measure?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo naturalEqualMeasureInfo = new(new(GTypeNames.GNumber), new(GTypeNames.Measure), new(GTypeNames.GNumber), naturalEqualMeasure);
        PosibleOverloads = new() { compareNumbersInfo, compareMeasuresInfo, measureEqualNaturalInfo, naturalEqualMeasureInfo };
        OperationToken = "==";
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
        GSObject? compareNumbers(GSObject? a, GSObject? b)
        {
            var result = (GSNumber?)a != (GSNumber?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo compareNumbersInfo = BinaryOverloadInfo.GetArithmetic(compareNumbers);
        GSObject? compareMeasures(GSObject? a, GSObject? b)
        {
            var result = (Measure?)a != (Measure?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo compareMeasuresInfo = new(new(GTypeNames.Measure), new(GTypeNames.GNumber), compareMeasures);
        GSObject? measureUnequalNatural(GSObject? a, GSObject? b)
        {
            var result = (Measure?)a != (GSNumber?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo measureUnequalNaturalInfo = new(new(GTypeNames.Measure), new(GTypeNames.GNumber), new(GTypeNames.GNumber), measureUnequalNatural);

        GSObject? naturalUnequalMeasure(GSObject? a, GSObject? b)
        {
            var result = (GSNumber?)a != (Measure?)b;
            return result ? (GSNumber)1 : (GSNumber)0;
        }
        BinaryOverloadInfo naturalUnequalMeasureInfo = new(new(GTypeNames.GNumber), new(GTypeNames.Measure), new(GTypeNames.GNumber), naturalUnequalMeasure);
        PosibleOverloads = new() { compareNumbersInfo, compareMeasuresInfo, measureUnequalNaturalInfo, naturalUnequalMeasureInfo };
        OperationToken = "!=";
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
        GSObject? numberSum(GSObject? a, GSObject? b) => (GSNumber?)a + (GSNumber?)b;
        BinaryOverloadInfo numberSumInfo = BinaryOverloadInfo.GetArithmetic(numberSum);
        GSObject? sequenceConcat(GSObject? a, GSObject? b)
        {
            ISequenciable? left = (ISequenciable?)a;
            if (left is null)
                return null;
            ISequenciable? right = (ISequenciable?)b;
            ISequenciable result = left.AttachSequence(right);
            
            return new GSequence(result);
        }
        GSharpType concatType(GSharpType left, GSharpType right) => left;
        BinaryOverloadInfo sequenceConcatInfo = new(new(GTypeNames.GSequence), new(GTypeNames.GSequence), sequenceConcat, concatType);
        GSObject? measureSum(GSObject? a, GSObject? b) => (Measure?)a + (Measure?)b;
        BinaryOverloadInfo measureSumInfo = new(new(GTypeNames.Measure), new(GTypeNames.Measure), measureSum);
        PosibleOverloads = new() { numberSumInfo , sequenceConcatInfo, measureSumInfo};
        OperationToken = "+";
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
        GSObject? func(GSObject? a, GSObject? b) => (GSNumber?)a - (GSNumber?)b;
        BinaryOverloadInfo info = BinaryOverloadInfo.GetArithmetic(func);
        GSObject? measureRest(GSObject? a, GSObject? b) => (Measure?)a - (Measure?)b;
        BinaryOverloadInfo measureSumInfo = new(new(GTypeNames.Measure), new(GTypeNames.Measure), measureRest);
        PosibleOverloads = new() { info };
        OperationToken = "-";
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
        GSObject? multiplyNumbers(GSObject? a, GSObject? b) => (GSNumber?)a * (GSNumber?)b;
        BinaryOverloadInfo multiplyNumbersInfo = BinaryOverloadInfo.GetArithmetic(multiplyNumbers);

        GSObject? measureTimesNatural(GSObject? a, GSObject? b) => (Measure?)a * (GSNumber?)b;
        BinaryOverloadInfo measureTimesNaturalInfo = new(new(GTypeNames.Measure), new(GTypeNames.GNumber), new(GTypeNames.Measure), measureTimesNatural);
        
        GSObject? naturalTimesMeasure(GSObject? a, GSObject? b) => (GSNumber?)a * (Measure?)b;
        BinaryOverloadInfo naturalTimesMeasureInfo = new(new(GTypeNames.GNumber), new(GTypeNames.Measure), new(GTypeNames.Measure), naturalTimesMeasure);
        
        PosibleOverloads = new() { multiplyNumbersInfo,  measureTimesNaturalInfo, naturalTimesMeasureInfo};
        OperationToken = "*";
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
        GSObject? divideNumbers(GSObject? a, GSObject? b) => GSObject.ToValueOfTruth(b) == 0 ?
                                                        throw new DefaultError("Atempted to divide by 0", "arithmetic")
                                                        : (GSNumber?)a / (GSNumber?)b;
        BinaryOverloadInfo divideNumbersInfo = BinaryOverloadInfo.GetArithmetic(divideNumbers);

        GSObject? divideMeasures(GSObject? a, GSObject? b) => GSObject.ToValueOfTruth(b) == 0 ?
                                                        throw new DefaultError("Atempted to divide by 0", "arithmetic")
                                                        : (Measure?)a / (Measure?)b;
        BinaryOverloadInfo divideMeasuresInfo = new(new(GTypeNames.Measure), new(GTypeNames.Measure), divideMeasures);
        PosibleOverloads = new() { divideNumbersInfo, divideMeasuresInfo};
        OperationToken = "/";
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
        GSObject? func(GSObject? a, GSObject? b) => GSObject.ToValueOfTruth(b) == 0 ? 
                                                    throw new DefaultError("Atempted to divide by 0", "arithmetic") 
                                                    : (GSNumber?)a % (GSNumber?)b;
        BinaryOverloadInfo info = BinaryOverloadInfo.GetArithmetic(func);
        PosibleOverloads = new() { info };
        OperationToken = "%";
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
        GSObject? func(GSObject? a, GSObject? b)
        {
            GSNumber? left = (GSNumber?)a;
            GSNumber? right = (GSNumber?)b;
            if (left is null || right is null)
                return null;
            return (GSNumber?)Math.Pow(left, right);
        }
        BinaryOverloadInfo info = BinaryOverloadInfo.GetArithmetic(func);
        PosibleOverloads = new() { info };
        OperationToken = "^";
    }
}
#endregion
