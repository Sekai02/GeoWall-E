namespace GeoWallECompiler.StandardLibrary;

public class PointGetter : ICallable
{    public GSObject Evaluate(Evaluator evaluator, List<GSObject?> arguments)
    {
        if (arguments.Count != 2)
            throw new SemanticError("Function 'point'", "2 arguments", $"{arguments.Count}");
        GSObject? argument1 = arguments[0];
        GSObject? argument2 = arguments[1];
        if (argument1 is null || argument2 is null)
            throw new DefaultError("Function 'point' cannot take an undefined argument");
        if (argument1 is GSNumber x && argument2 is GSNumber y)
            return new GSPoint(x, y);
        if (argument1 is not GSPoint)
            throw new SemanticError("Function 'point'", "number", argument1.GetType().Name);
        else
            throw new SemanticError("Function 'point'", "number", argument2.GetType().Name);

    }
    public int GetArgumentsAmount() => 2;
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.Line);
}
public class LineGetter : ICallable
{
    public GSObject Evaluate(Evaluator evaluator, List<GSObject?> arguments)
    {
        if (arguments.Count != 2)
            throw new SemanticError("Function 'Line'", "2 arguments", $"{arguments.Count}");
        GSObject? argument1 = arguments[0];
        GSObject? argument2 = arguments[1];
        if(argument1 is null || argument2 is null)
            throw new DefaultError("Function 'Line' cannot take an undefined argument");
        if (argument1 is GSPoint point1 && argument2 is GSPoint point2)
            return new Line(point1, point2);
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
    public GSObject Evaluate(Evaluator evaluator, List<GSObject?> arguments)
    {
        if (arguments.Count != 2)
            throw new SemanticError("Function 'Segment'", "2 arguments", $"{arguments.Count}");
        GSObject? argument1 = arguments[0];
        GSObject? argument2 = arguments[1];
        if (argument1 is null || argument2 is null)
            throw new DefaultError("Function 'Segment' cannot take an undefined argument");
        if (argument1 is GSPoint point1 && argument2 is GSPoint point2)
            return new Segment(point1, point2);
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
    public GSObject Evaluate(Evaluator evaluator, List<GSObject?> arguments)
    {
        if (arguments.Count != 2)
            throw new SemanticError("Function 'Ray'", "2 arguments", $"{arguments.Count}");
        GSObject? argument1 = arguments[0];
        GSObject? argument2 = arguments[1];
        if (argument1 is null || argument2 is null)
            throw new DefaultError("Function 'Ray' cannot take an undefined argument");
        if (argument1 is GSPoint point1 && argument2 is GSPoint point2)
            return new Ray(point1, point2);
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
    public GSObject Evaluate(Evaluator evaluator, List<GSObject?> arguments)
    {
        if (arguments.Count != 2)
            throw new SemanticError("Function 'Circle'", "2 arguments", $"{arguments.Count}");
        GSObject? argument1 = arguments[0];
        GSObject? argument2 = arguments[1];
        if (argument1 is null || argument2 is null)
            throw new DefaultError("Function 'Circle' cannot take an undefined argument");
        if (argument1 is GSPoint center && argument2 is Measure radius)
            return new Circle(center, radius);
        if (argument1 is not GSPoint)
            throw new SemanticError("Function 'Circle'", "point", argument1.GetType().Name);
        else
            throw new SemanticError("Function 'Circle'", "measure", argument2.GetType().Name);
    }
    public int GetArgumentsAmount() => 2;
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.Circle);
}
public class ArcGetter: ICallable
{
    public GSObject Evaluate(Evaluator evaluator, List<GSObject?> arguments)
    {
        if (arguments.Count != 4)
            throw new SemanticError("Function 'Circle'", "2 arguments", $"{arguments.Count}");
        GSObject? argument1 = arguments[0];
        GSObject? argument2 = arguments[1];
        GSObject? argument3 = arguments[2];
        GSObject? argument4 = arguments[3];
        if (argument1 is null || argument2 is null || argument3 is null || argument4 is null)
            throw new DefaultError("Function 'Circle' cannot take an undefined argument");
        if (argument1 is GSPoint center && argument2 is GSPoint point1 && argument3 is GSPoint point2 && argument4 is Measure radius)
            return new Arc(center, point1, point2, radius);
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
    public GSharpType GetType(TypeChecker checker, List<GSharpType> argumentsTypes) => new(GTypeNames.Arc);
}
