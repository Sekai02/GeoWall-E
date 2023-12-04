namespace GeoWallECompiler;
public class ConstantsDeclaration : Statement
{
    public override void Accept(IStatementVisitor visitor) => visitor.VisitConstantDeclaration(this);
    public ConstantsDeclaration(List<string> constantNames, GSharpExpression value)
    {
        ConstantNames = constantNames;
        ValueExpression = value;
    }
    public List<string> ConstantNames { get; }
    public GSharpExpression ValueExpression { get; }
}
public interface ICallable
{
    public int GetArgumentsAmount();
    GSObject? Evaluate(Evaluator evaluator, List<GSObject?> arguments);
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
    public DeclaredFunction(FunctionDeclaration declaration) => Declaration = declaration;
    public FunctionDeclaration Declaration { get; }
    public GSObject? Evaluate(Evaluator evaluator, List<GSObject?> arguments)
    {
        Context<GSObject?, ICallable> functionContext = new(evaluator.EvaluationContext);
        for (int i = 0; i < Declaration.Parameters.Count; i++)
        {
            functionContext.SetVariable(Declaration.Parameters[i], arguments[i]);
        }
        Context<GSObject?, ICallable> previous = evaluator.EvaluationContext;
        evaluator.EvaluationContext = functionContext;
        GSObject? result = Declaration.Body.Accept(evaluator);
        evaluator.EvaluationContext = previous;
        return result;
    } 
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes)
    {
        Context<GSharpType, ICallable> functionContext = new(checker.TypeEnvironment);
        for (int i = 0; i < Declaration.Parameters.Count; i++)
        {
            functionContext.SetVariable(Declaration.Parameters[i], argumentsTypes[i]);
        }
        Context<GSharpType, ICallable> previous = checker.TypeEnvironment;
        checker.TypeEnvironment = functionContext;
        GSharpType result = Declaration.Body.Accept(checker);
        checker.TypeEnvironment = previous;
        return result;
    }
    public int GetArgumentsAmount() => Declaration.Parameters.Count;
}
