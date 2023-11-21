namespace GeoWallECompiler;

public class EvaluationContext
{
    public EvaluationContext? Enclosing { get; private set; }
    private Dictionary<string, GSharpObject> Variables;
    private Dictionary<string, DeclaredFunction> Functions;
    public EvaluationContext() {
        Variables = new();
        Functions = new();
    }
    public EvaluationContext(EvaluationContext? enclosing)
    {
        Enclosing = enclosing;
        Variables = new();
        Functions = new();
    }
    public GSharpObject AccessVariable(string variableName)
    {
        if (!Variables.TryGetValue(variableName, out GSharpObject? value)) 
        {
            if(Enclosing is not null)
                return Enclosing.AccessVariable(variableName);
            ErrorHandler.AddError(new DefaultError($"Variable {variableName} not found"));
        }
        return value;
    }
    public void SetVariable(string variableName, GSharpObject variableValue)
    {
        if (!Variables.TryAdd(variableName, variableValue))
            ErrorHandler.AddError(new DefaultError($"Variable {variableName} already exist"));
    }
    public DeclaredFunction AccessFunction(string functionName)
    {
        if (!Functions.TryGetValue(functionName, out DeclaredFunction? function))
        {
            if (Enclosing is not null)
                return Enclosing.AccessFunction(functionName);
            ErrorHandler.AddError(new DefaultError($"Function {functionName} not found"));
        }
        return function;
    }
    public void SetFunction(string functionName, DeclaredFunction function)
    {
        if (!Functions.TryAdd(functionName, function))
            ErrorHandler.AddError(new DefaultError($"Function {function} already exist"));
    }
    public GSharpObject AccessVariableAt(int distance, string name)
    {
        EvaluationContext? ancestor = this;
        for (int i = 0; i < distance; i--)
            ancestor = ancestor?.Enclosing;
        return ancestor?.AccessVariable(name);
    }
    public DeclaredFunction AccessFunctionAt(int distance, string name)
    {
        EvaluationContext? ancestor = this;
        for (int i = 0; i < distance; i--)
            ancestor = ancestor?.Enclosing;
        return ancestor?.AccessFunction(name);
    }
}
public class Scope
{
    public Dictionary<string, bool> Variables;
    public Dictionary<string, bool> Functions;
    public Scope()
    {
        Variables = new();
        Functions = new();
    }
}
