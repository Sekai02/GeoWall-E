namespace GeoWallECompiler.StandardLibrary;
public class LineGetter : ICallable
{
    public GSObject Evaluate(Evaluator evaluator, List<GSObject> arguments)
    {
        if (arguments.Count != 2)
            throw new SemanticError("Function 'Line'", "2 arguments", $"{arguments.Count}");
        GSObject argument1 = arguments[0];
        GSObject argument2 = arguments[1];
        if (argument1 is GSPoint && argument2 is GSPoint)
            return new Line((GSPoint?)argument1, (GSPoint?)argument2);
        if (argument1 is not GSPoint)
            throw new SemanticError("Function 'Line'", "point", argument1.GetType().Name);
        else
            throw new SemanticError("Function 'Line'", "point", argument2.GetType().Name);

    }
    public int GetArgumentsAmount() => 2;
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.Line);
}
public class SegmentGetter : ICallable
{
    public GSObject Evaluate(Evaluator evaluator, List<GSObject> arguments)
    {
        if (arguments.Count != 2)
            throw new SemanticError("Function 'Segment'", "2 arguments", $"{arguments.Count}");
        GSObject argument1 = arguments[0];
        GSObject argument2 = arguments[1];
        if (argument1 is GSPoint && argument2 is GSPoint)
            return new Segment((GSPoint?)argument1, (GSPoint?)argument2);
        if (argument1 is not GSPoint)
            throw new SemanticError("Function 'Segment'", "point", argument1.GetType().Name);
        else
            throw new SemanticError("Function 'Segment'", "point", argument2.GetType().Name);

    }
    public int GetArgumentsAmount() => 2;
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.Segment);
}
public class RayGetter: ICallable
{
    public GSObject Evaluate(Evaluator evaluator, List<GSObject> arguments)
    {
        if (arguments.Count != 2)
            throw new SemanticError("Function 'Ray'", "2 arguments", $"{arguments.Count}");
        GSObject argument1 = arguments[0];
        GSObject argument2 = arguments[1];
        if (argument1 is GSPoint && argument2 is GSPoint)
            return new Ray((GSPoint?)argument1, (GSPoint?)argument2);
        if (argument1 is not GSPoint)
            throw new SemanticError("Function 'Ray'", "point", argument1.GetType().Name);
        else
            throw new SemanticError("Function 'Ray'", "point", argument2.GetType().Name);
    }
    public int GetArgumentsAmount() => 2;
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.Ray);
}
public class CircleGetter : ICallable
{
    public GSObject Evaluate(Evaluator evaluator, List<GSObject> arguments)
    {
        if (arguments.Count != 2)
            throw new SemanticError("Function 'Circle'", "2 arguments", $"{arguments.Count}");
        GSObject argument1 = arguments[0];
        GSObject argument2 = arguments[1];
        if (argument1 is GSPoint && argument2 is Measure)
            return new Circle((GSPoint?)argument1, (Measure?)argument2);
        if (argument1 is not GSPoint)
            throw new SemanticError("Function 'Circle'", "point", argument1.GetType().Name);
        else
            throw new SemanticError("Function 'Circle'", "measure", argument2.GetType().Name);
    }
    public int GetArgumentsAmount() => 2;
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.Ray);
}
public class ArcGetter: ICallable
{
    public GSObject Evaluate(Evaluator evaluator, List<GSObject> arguments)
    {
        if (arguments.Count != 4)
            throw new SemanticError("Function 'Circle'", "2 arguments", $"{arguments.Count}");
        GSObject argument1 = arguments[0];
        GSObject argument2 = arguments[1];
        GSObject argument3 = arguments[2];
        GSObject argument4 = arguments[3];
        if (argument1 is GSPoint && argument2 is GSPoint && argument3 is GSPoint && argument4 is Measure)
            return new Arc((GSPoint?)argument1, (GSPoint?)argument2, (GSPoint?)argument3, (Measure?)argument4);
        if (argument1 is not GSPoint)
            throw new SemanticError("Function 'Circle'", "point", argument1.GetType().Name);
        if (argument2 is not GSPoint)
            throw new SemanticError("Function 'Circle'", "point", argument2.GetType().Name);
        if (argument3 is not GSPoint)
            throw new SemanticError("Function 'Circle'", "point", argument3.GetType().Name);
        else
            throw new SemanticError("Function 'Circle'", "measure", argument4.GetType().Name);
    }
    public int GetArgumentsAmount() => 2;
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.Ray);
}
