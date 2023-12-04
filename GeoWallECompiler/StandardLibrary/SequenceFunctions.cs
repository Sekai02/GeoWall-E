namespace GeoWallECompiler.StandardLibrary;

public class CountFunction : ICallable
{
    public GSObject? Evaluate(Evaluator evaluator, List<GSObject> arguments)
    {
        if (arguments.Count != 1)
            throw new SemanticError("Function 'count'", "1 argument", $"{arguments.Count}");
        GSObject argument = arguments[0];
        if (argument is ISequenciable sequenciable)
        {
            int? count = sequenciable.GetCount();
            return count is not null? (GSNumber)count : null;
        }
        else
            throw new SemanticError("Function 'count'", "sequence", argument.GetType().Name);
    }
    public int GetArgumentsAmount() => 1;
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.GNumber);
}
public class RandomsFunction : ICallable
{
    public GSObject? Evaluate(Evaluator evaluator, List<GSObject> arguments)
    {
        if (arguments.Count != 0)
            throw new SemanticError("Function 'randoms'", "0 arguments", $"{arguments.Count}");
        return GSequence.GetRandomNumbers();
    }

    public int GetArgumentsAmount() => 0;
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.GSequence, GTypeNames.GNumber);
}
public class PointsFunction : ICallable
{
    public GSObject? Evaluate(Evaluator evaluator, List<GSObject?> arguments)
    {
        if (arguments.Count != 1)
            throw new SemanticError("Function 'count'", "1 argument", $"{arguments.Count}");
        GSObject argument = arguments[0];
        if (argument is IDrawable drawable)
            return GSequence.GetRandomPoints(drawable);
        else
            throw new SemanticError("Function 'points'", "figure", argument.GetType().Name);
    }
    public int GetArgumentsAmount() => 1;
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.GSequence, GTypeNames.Point);
}
