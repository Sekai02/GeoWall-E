namespace GeoWallECompiler;
public class ConstantsDeclaration : Statement
{
    public override void Accept(IStatementVisitor visitor) => visitor.VisitConstantDeclaration(this);
    public ConstantsDeclaration(List<string> constantNames, GSharpExpression value)
    {
        ConstantNames = constantNames;
        Value = value;
    }
    public List<string> ConstantNames { get; }
    public GSharpExpression Value { get; }
}
public interface ICallable
{
    public int GetArgumentsAmount();
    GSharpObject Evaluate(Evaluator evaluator, List<GSharpObject> arguments);
    GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes);
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
    public DeclaredFunction(FunctionDeclaration declaration)
    {
        Declaration = declaration;
    }
    public FunctionDeclaration Declaration { get; }
    public GSharpObject Evaluate(Evaluator evaluator, List<GSharpObject> arguments)
    {
        Context<GSharpObject, DeclaredFunction> functionContext = new(evaluator.EvaluationContext);
        for (int i = 0; i < Declaration.Parameters.Count; i++)
        {
            functionContext.SetVariable(Declaration.Parameters[i], arguments[i]);
        }
        Context<GSharpObject, DeclaredFunction> previous = evaluator.EvaluationContext;
        evaluator.EvaluationContext = functionContext;
        GSharpObject result = Declaration.Body.Accept(evaluator);
        evaluator.EvaluationContext = previous;
        return result;
    } 
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes)
    {
        Context<GSharpType, DeclaredFunction> functionContext = new(checker.TypeEnvironment);
        for (int i = 0; i < Declaration.Parameters.Count; i++)
        {
            functionContext.SetVariable(Declaration.Parameters[i], argumentsTypes[i]);
        }
        Context<GSharpType, DeclaredFunction> previous = checker.TypeEnvironment;
        checker.TypeEnvironment = functionContext;
        GSharpType result = Declaration.Body.Accept(checker);
        checker.TypeEnvironment = previous;
        return result;
    }
    public int GetArgumentsAmount() => Declaration.Parameters.Count;
}
