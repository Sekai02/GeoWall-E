namespace GeoWallECompiler;
public class IfThenElseStatement: GSharpExpression
{
    public IfThenElseStatement(GSharpExpression Cond, GSharpExpression IfExp, GSharpExpression ElseExp)
    {
        Condition = Cond;
        IfExpression = IfExp;
        ElseExpression = ElseExp;
    }
    #region Methods
    public override GSharpObject GetValue() => Result(Condition, IfExpression, ElseExpression);
    public override GSharpTypes CheckType()
    {
        Condition.CheckType();
        GSharpTypes ifType = IfExpression.CheckType();
        GSharpTypes elseType = ElseExpression.CheckType();
        return ifType == elseType ? ifType : throw new DefaultError("SemanticError", "if-then-else statements must have the same return type");
    }
    private GSharpObject Result(GSharpExpression Cond, GSharpExpression IfExp, GSharpExpression ElseExp)
    {
        bool condition = Cond.GetValue().ToValueOfTruth() == 1;
        if (condition)
            return IfExp.GetValue();
        return ElseExp.GetValue();
    }
    #endregion
    #region Properties
    public GSharpExpression Condition { get; protected set; }
    public GSharpExpression IfExpression { get; protected set; }
    public GSharpExpression ElseExpression { get; protected set; }
    #endregion
}
