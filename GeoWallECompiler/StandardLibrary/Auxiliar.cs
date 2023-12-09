namespace GeoWallECompiler.StandardLibrary;
public class MeasureFunction : ICallable
{
    public GSObject Evaluate(Evaluator evaluator, List<GSObject?> arguments)
    {
        if (arguments.Count != 2)
            throw new SemanticError("Function 'measure'", "2 arguments", $"{arguments.Count}");
        GSObject? argument1 = arguments[0];
        GSObject? argument2 = arguments[1];
        if (argument1 is null || argument2 is null)
            throw new DefaultError("Function 'Segment' cannot take an undefined argument");
        if (argument1 is GSPoint point1 && argument2 is GSPoint point2)
            return new Measure(point1, point2);
        if (argument1 is not GSPoint)
            throw new SemanticError("Function 'measure'", "point", argument1.GetType().Name);
        else
            throw new SemanticError("Function 'measure'", "point", argument2.GetType().Name);
    }
    public int GetArgumentsAmount() => 2;
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.Measure);
}
