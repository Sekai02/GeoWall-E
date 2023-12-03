namespace GeoWallECompiler.StandardLibrary;
public class MeasureFunction : ICallable
{
    public GSObject Evaluate(Evaluator evaluator, List<GSObject> arguments)
    {
        if (arguments.Count != 2)
            throw new SemanticError("Function 'measure'", "2 arguments", $"{arguments.Count}");
        GSObject argument1 = arguments[0];
        GSObject argument2 = arguments[1];
        if (argument1 is GSPoint && argument2 is GSPoint)
            return new Measure((GSPoint?)argument1, (GSPoint?)argument2);
        if (argument1 is not GSPoint)
            throw new SemanticError("Function 'measure'", "point", argument1.GetType().Name);
        else
            throw new SemanticError("Function 'measure'", "point", argument2.GetType().Name);
    }
    public int GetArgumentsAmount() => 2;
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.Measure);
}
