namespace GeoWallECompiler;
public class Context
{
    public Context? Enclosing { get; private set; }
    private Dictionary<string, GSharpObject> Variables;
    public Context() => Variables = new();
    public Context(Context enclosing)
    {
        Enclosing = enclosing;
        Variables = new();
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
}
