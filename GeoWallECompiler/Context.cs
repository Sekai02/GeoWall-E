using GeoWallECompiler.StandardLibrary;
using System.Runtime.InteropServices;

namespace GeoWallECompiler;

public class Context<Var, Fun>
{
    public Context<Var,Fun>? Enclosing { get; private set; }
    private Dictionary<string, Var> Variables;
    private Dictionary<string, Fun> Functions;
    public Context() {
        Variables = new();
        Functions = new();
    }
    public Context(Context<Var, Fun>? enclosing)
    {
        Enclosing = enclosing;
        Variables = new();
        Functions = new();
    }
    public Var AccessVariable(string variableName)
    {
        if (!Variables.TryGetValue(variableName, out Var? value))
        {
            if (Enclosing is not null)
                return Enclosing.AccessVariable(variableName);
            ErrorHandler.AddError(new DefaultError($"Variable {variableName} not found"));
        }
        return value;
    }
    public void SetVariable(string variableName, Var variableValue)
    {
        if (variableName == "_")
            return;
        if (!Variables.TryAdd(variableName, variableValue))
            ErrorHandler.AddError(new DefaultError($"Variable {variableName} already exist"));
    }
    public Fun AccessFunction(string functionName)
    {
        if (!Functions.TryGetValue(functionName, out Fun? function))
        {
            if (Enclosing is not null)
                return Enclosing.AccessFunction(functionName);
            ErrorHandler.AddError(new DefaultError($"Function {functionName} not found"));
        }
        return function;
    }
    public void SetFunction(string functionName, Fun function)
    {
        if (functionName == "_")
            return;
        if (!Functions.TryAdd(functionName, function))
            ErrorHandler.AddError(new DefaultError($"Function {function} already exist"));
    }
    public Var AccessVariableAt(int distance, string name)
    {
        Context<Var,Fun>? ancestor = this;
        if(distance == 0)
            return ancestor.AccessVariable(name);
        for (int i = 0; i < distance; i++)
            ancestor = ancestor?.Enclosing;
        return ancestor.AccessVariable(name);
    }
    public Fun AccessFunctionAt(int distance, string name)
    {
        Context<Var,Fun>? ancestor = this;
        for (int i = 0; i < distance; i++)
            ancestor = ancestor?.Enclosing;
        return ancestor.AccessFunction(name);
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
