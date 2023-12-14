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
            throw new SemanticError("Function 'intersect'", "2 arguments", $"{arguments.Count}");
        GSObject? argument1 = arguments[0];
        GSObject? argument2 = arguments[1];
        if (argument1 is null || argument2 is null)
            throw new DefaultError("Function 'intersect' cannot take an undefined argument");
        if (argument1 is IDrawable figure1 && argument2 is IDrawable figure2)
        {
            if(argument1 is GSPoint point1 && argument2 is GSPoint point2)
            {
                List<GSPoint> result = new();
                if (point1.Coordinates.X == point2.Coordinates.X && point1.Coordinates.Y == point2.Coordinates.Y)
                    result.Add(point1);
                return new GSequence(result, result.Count);
            }
            if (argument1 is GSPoint p1)
            {
                List<GSPoint> result = new();
                Equation equation = figure2.Equation;
                if (EquationSolver.EvaluateEcuation(p1.Coordinates.X, p1.Coordinates.Y, equation) == 0)
                    result.Add(p1);
                return new GSequence(result, result.Count);
            }
            if (argument2 is GSPoint p2)
            {
                List<GSPoint> result = new();
                Equation equation = figure1.Equation;
                if (EquationSolver.EvaluateEcuation(p2.Coordinates.X, p2.Coordinates.Y, equation) == 0)
                    result.Add(p2);
                return new GSequence(result, result.Count);
            }
            Equation equation1 = figure1.Equation;
            Equation equation2 = figure2.Equation;
            List<GSPoint> points = new(EquationSolver.SolveCircularSystem(equation1, equation2));
            return new GSequence(points, points.Count);            
        }
        if (argument1 is not IDrawable)
            throw new SemanticError("Function 'intersect'", "figure", argument1.GetType().Name);
        else
            throw new SemanticError("Function 'intersect'", "figure", argument2.GetType().Name);
    }
    public int GetArgumentsAmount() => 2;
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.GSequence, GTypeNames.Point);
}
