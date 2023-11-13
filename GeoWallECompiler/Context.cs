namespace GeoWallECompiler;
public class Context
{
    public Context? Enclosing { get; private set; }
    private Dictionary<string, GSharpObject> Variables;
    private Dictionary<string, DeclaredFunction> Functions;
    public Context() {
        Variables = new();
        Functions = new();
    }
    public Context(Context enclosing)
    {
        Enclosing = enclosing;
        Variables = new();
        Functions = new();
    }
    public GSharpObject GetVariableValue(string variableName)
    {
        if (!Variables.TryGetValue(variableName, out GSharpObject? value)) 
        {
            if(Enclosing is not null)
                return Enclosing.GetVariableValue(variableName);
            ErrorHandler.AddError(new DefaultError($"Variable {variableName} not found"));
        }
        return value;
    }
    public void DefineVariable(string variableName, GSharpObject variableValue)
    {
        if (!Variables.TryAdd(variableName, variableValue))
            ErrorHandler.AddError(new DefaultError($"Variable {variableName} already exist"));
    }
    public DeclaredFunction GetFunction(string functionName)
    {
        if (!Functions.TryGetValue(functionName, out DeclaredFunction? function))
        {
            if (Enclosing is not null)
                return Enclosing.GetFunction(functionName);
            ErrorHandler.AddError(new DefaultError($"Function {functionName} not found"));
        }
        return function;
    }
    public void DefineFunction(string functionName, DeclaredFunction function)
    {
        if (!Functions.TryAdd(functionName, function))
            ErrorHandler.AddError(new DefaultError($"Function {function} already exist"));
    }
}
