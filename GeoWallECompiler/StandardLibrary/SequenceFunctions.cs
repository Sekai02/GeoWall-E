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
            throw new SemanticError("Function 'points'", "1 argument", $"{arguments.Count}");
        GSObject argument = arguments[0];
        if (argument is IDrawable drawable)
            return GSequence.GetRandomPoints(drawable);
        else
            throw new SemanticError("Function 'points'", "figure", argument.GetType().Name);
    }
    public int GetArgumentsAmount() => 1;
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.GSequence, GTypeNames.Point);
}
public class SamplesFunction : ICallable
{
    public GSObject? Evaluate(Evaluator evaluator, List<GSObject?> arguments) 
    {
        if (arguments.Count != 0)
            throw new SemanticError("Function 'randoms'", "0 arguments", $"{arguments.Count}");
        return GSequence.GetRandomPoints();
    }
    public int GetArgumentsAmount() => throw new NotImplementedException();
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.GSequence, GTypeNames.Point);
}
public class IntersectFunction : ICallable
{
    public GSObject? Evaluate(Evaluator evaluator, List<GSObject?> arguments)
    {
        if (arguments.Count != 2)
            throw new SemanticError("Function 'intersec'", "2 arguments", $"{arguments.Count}");
        GSObject? argument1 = arguments[0];
        GSObject? argument2 = arguments[1];
        if (argument1 is null || argument2 is null)
            throw new DefaultError("Function 'intersect' cannot take an undefined argument");
        if (argument1 is IDrawable figure1 && argument2 is IDrawable figure2)
        {
            Equation equation1 = figure1.Equation;
            Equation equation2 = figure2.Equation;
            List<GSPoint> points = new(EquationSolver.SolveCircularSystem(equation1, equation2));
            return new GSequence(points, points.Count);            
        }
        if (argument1 is not GSPoint)
            throw new SemanticError("Function 'Segment'", "point", argument1.GetType().Name);
        else
            throw new SemanticError("Function 'Segment'", "point", argument2.GetType().Name);
    }
    public int GetArgumentsAmount() => 2;
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.GSequence, GTypeNames.Point);
}
