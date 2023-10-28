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
    public override object GetValue() => Result(Condition, IfExpression, ElseExpression);
    public override GSharpTypes CheckType()
    {
        Condition.CheckType();
        GSharpTypes ifType = IfExpression.CheckType();
        GSharpTypes elseType = ElseExpression.CheckType();
        return ifType == elseType ? ifType : throw new DefaultError("SemanticError", "if-then-else statements must have the same return type");
    }
    private object Result(GSharpExpression Cond, GSharpExpression IfExp, GSharpExpression ElseExp)
    {
        if (Cond.GetValue() is not bool) //cambiar esto por algo equivalente a bool
            throw new SemanticError("if-else condition", "boolean", Cond.GetValue().GetType().Name);
        else
        {
            bool condition = (bool)Cond.GetValue();
            if (condition)
                return IfExp.GetValue();
            if (IfExp != null)
                if (ElseExp == null)
                    return null;
            return ElseExp.GetValue();
        }
    }
    #endregion
    #region Properties
    public GSharpExpression Condition { get; protected set; }
    public GSharpExpression IfExpression { get; protected set; }
    public GSharpExpression ElseExpression { get; protected set; }
    #endregion
}
