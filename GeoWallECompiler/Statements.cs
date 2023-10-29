namespace GeoWallECompiler;

/// <summary>
/// Clase que representa a una expresion de la forma if [condicion] then [expresion] else [expresion]
/// </summary>
public class IfThenElseStatement: GSharpExpression
{
    /// <summary>
    /// Crea una instancia de una expresion condicional
    /// </summary>
    /// <param name="Cond">Expresion condicional</param>
    /// <param name="IfExp">Expresion a efectuar si se cumple la condicion</param>
    /// <param name="ElseExp">Expresion a efectuar si no se cumple la condicion</param>
    public IfThenElseStatement(GSharpExpression Cond, GSharpExpression IfExp, GSharpExpression ElseExp)
    {
        Condition = Cond;
        IfExpression = IfExp;
        ElseExpression = ElseExp;
    }
    #region Methods
    public override GSharpObject GetValue() => Result(Condition, IfExpression, ElseExpression);
    /// <summary>
    /// Chequea si los tipos de las partes de la expresion condicional son correctos. 
    /// Devuelve el tipo de retorno de la expresion solo si los valores if y else son los mismos
    /// </summary>
    /// <returns>Tipo de retorno de la expresion condicional</returns>
    /// <exception cref="DefaultError"></exception>
    public override GSharpTypes CheckType()
    {
        Condition.CheckType();
        GSharpTypes ifType = IfExpression.CheckType();
        GSharpTypes elseType = ElseExpression.CheckType();
        return ifType == elseType ? ifType : throw new DefaultError("SemanticError", "if-then-else statements must have the same return type");
    }
    /// <summary>
    /// Evalua la expresion condicional, devolviendo el valor de IfExp si se cumple Cond y ElseExp en caso contrario
    /// </summary>
    /// <param name="Cond">Condicion</param>
    /// <param name="IfExp">Expresion a efectuar si se cumple la condicion</param>
    /// <param name="ElseExp">Expresion a efectuar si no se cumple la condicion</param>
    /// <returns></returns>
    private GSharpObject Result(GSharpExpression Cond, GSharpExpression IfExp, GSharpExpression ElseExp)
    {
        bool condition = Cond.GetValue().ToValueOfTruth() == 1;
        if (condition)
            return IfExp.GetValue();
        return ElseExp.GetValue();
    }
    #endregion
    #region Properties
    /// <summary>
    /// Condicion que se debe cumplir
    /// </summary>
    public GSharpExpression Condition { get; protected set; }
    /// <summary>
    /// Expresion a efectuar si se cumple la condicion
    /// </summary>
    public GSharpExpression IfExpression { get; protected set; }
    /// <summary>
    /// Expresion a efectuar si no se cumple la condicion
    /// </summary>
    public GSharpExpression ElseExpression { get; protected set; }
    #endregion
}
