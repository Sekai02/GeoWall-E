namespace GeoWallECompiler;
public class ConstantsDeclaration : Statement
{
    public override void Accept(IStatementVisitor visitor) => visitor.VisitConstantDeclaration(this);
    public ConstantsDeclaration(List<string> constantNames, GSharpExpression value)
    {
        ConstantNames = constantNames;
        Value = value;
        //if (constantNames.Count == 1)
        //{
        //    Constant constant = new(constantNames[0], value);
        //    DeclaredConstants = new() { constant };
        //}
        //else if (value.GetValue() is not GSharpSequence)
        //    throw new SemanticError("Match declaration", "squence", value.CheckType().ToString());
        //else
        //{
        //    //el llamado siguiente a get value puede traer problemas 
        //    GSharpSequence? sequence = value.GetValue() as GSharpSequence;
        //    List<Constant> declaredConstants = new();
        //    int index = 0;
        //    foreach(GSharpObject obj in sequence.Sequence)
        //    {
        //        if(index == constantNames.Count - 1)
        //        {
        //            Literal constantValue = new(new GSharpSequence(sequence, index));
        //            declaredConstants.Add(new Constant(constantNames[index], constantValue));
        //            break;
        //        }
        //        declaredConstants.Add(new Constant(constantNames[index], new Literal(obj)));
        //        index++;
        //    }
        //    if(index < constantNames.Count - 1)
        //    {
        //        for(int i = index; index < constantNames.Count; i++)
        //            declaredConstants.Add(new Constant(constantNames[index], null));
        //    }
        //    DeclaredConstants = declaredConstants;
        //}
    }
    public List<string> ConstantNames { get; }
    public GSharpExpression Value { get; }
}
public interface ICallable
{
    public int GetArgumentsAmount();
    GSharpObject Evaluate(Evaluator evaluator, List<GSharpObject> arguments);
}
public class FunctionDeclaration : Statement
{
    public FunctionDeclaration(string name, List<string> parameters, GSharpExpression body)
    {
        Name = name;
        Parameters = parameters;
        Body = body;
    }
    public override void Accept(IStatementVisitor visitor) => visitor.VisitFunctionDeclaration(this);
    public string Name { get; }
    public List<string> Parameters { get; }
    public GSharpExpression Body { get; }

}
public class DeclaredFunction : ICallable
{
    public DeclaredFunction (FunctionDeclaration declaration)
    {
        Declaration = declaration;
    }
    public FunctionDeclaration Declaration { get; }
    public GSharpObject Evaluate(Evaluator evaluator, List<GSharpObject> arguments) 
    {
        Context functionContext = new(evaluator.EvaluationContext);
        for(int i = 0; i < Declaration.Parameters.Count; i++)
        {
            functionContext.DefineVariable(Declaration.Parameters[i], arguments[i]);
        }
        Context previous = evaluator.EvaluationContext;
        evaluator.EvaluationContext = functionContext;
        GSharpObject result = Declaration.Body.Accept(evaluator);
        evaluator.EvaluationContext = previous;
        return result;
    }
    public int GetArgumentsAmount() => Declaration.Parameters.Count;
}
