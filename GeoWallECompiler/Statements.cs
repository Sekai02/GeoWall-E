namespace GeoWallECompiler;

/// <summary>
/// Clase que representa a una expresion de la forma if [condicion] then [expresion] else [expresion]
/// </summary>
public class IfThenElse: GSharpExpression
{

    /// <summary>
    /// Crea una instancia de una expresion condicional
    /// </summary>
    /// <param name="Cond">Expresion condicional</param>
    /// <param name="IfExp">Expresion a efectuar si se cumple la condicion</param>
    /// <param name="ElseExp">Expresion a efectuar si no se cumple la condicion</param>
    public IfThenElse(GSharpExpression Cond, GSharpExpression IfExp, GSharpExpression ElseExp)
    {
        Condition = Cond;
        IfExpression = IfExp;
        ElseExpression = ElseExp;
    }
    public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.visitIfThenElse(this);
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
/// <summary>
/// Representa las expresiones let-in.
/// </summary>
public class LetIn : GSharpExpression
{
    /// <summary>
    /// Construye una expresion let-in
    /// </summary>
    /// <param name="declaredConstants">Variables locales de la expresion let-in</param>
    /// <param name="body">Cuerpo de la expresion let-in</param>
    public LetIn(Dictionary<string, Constant> declaredConstants, GSharpExpression body)
    {
        DeclaredConstants = declaredConstants;
        Body = body;
    }
    public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.visitLetIn(this);
    /// <summary>
    /// Variables locales de la expresion let-in
    /// </summary>
    public Dictionary<string, Constant> DeclaredConstants { get; private set; }
    /// <summary>
    /// Cuerpo de la expresion let-in
    /// </summary>
    public GSharpExpression Body { get; private set; }
}
