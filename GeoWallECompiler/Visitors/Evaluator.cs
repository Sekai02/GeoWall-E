using GeoWallECompiler.Expressions;

namespace GeoWallECompiler;
public class Evaluator : IExpressionVisitor<GSObject>, IStatementVisitor
{
    public Dictionary<GSharpExpression, int> References = new();
    private Context<GSObject, DeclaredFunction>? environment = new();
    private IWalleUI UserInterface;
    public Evaluator(IDrawer drawer)
    {
        environment = new();
        Drawer = drawer;
    }
    public Context<GSObject, DeclaredFunction>? EvaluationContext { get => environment; set => environment = value; }
    public IDrawer Drawer { get; }
    public void ResolveReference(GSharpExpression expression, int depth)
    {
        References.Add(expression, depth);
    }
    public GSObject VisitBinaryOperation(BinaryOperation binary)
    {
        GSObject left = binary.LeftArgument.Accept(this);
        GSObject right = binary.RightArgument.Accept(this);
        if (left.GetType() == binary.AcceptedType && right.GetType() == binary.AcceptedType)
            return binary.Operation(left, right);
        var conflictiveType = left.GetType() != binary.AcceptedType ? left.GetType().Name : right.GetType().Name;
        throw new SemanticError($"Operator `{binary.OperationToken}`", binary.ReturnedType.ToString(), conflictiveType);
    }
    public GSObject VisitConstant(Constant constant)
    {
        int distance = References[constant];
        return environment.AccessVariableAt(distance, constant.Name);
    }
    public GSObject VisitFunctionCall(FunctionCall functionCall)
    {
        int distance = References[functionCall];
        DeclaredFunction callee = environment.AccessFunctionAt(distance, functionCall.FunctionName);
        List<GSObject> arguments = new();
        foreach (GSharpExpression argument in functionCall.Arguments)
        {
            arguments.Add(argument.Accept(this));
        }
        return callee.Evaluate(this, arguments);
    }
    public void VisitConstantDeclaration(ConstantsDeclaration declaration)
    {
        GSObject value = declaration.ValueExpression.Accept(this);
        List<string> constantNames = declaration.ConstantNames;
        if (value == null)
        {
            ErrorHandler.AddError(new SyntaxError("value", "declaration"));
            return;
        }
        if (constantNames.Count == 1)
        {
            environment.SetVariable(constantNames[0], value);
            return;
        }
        if (value is not GSharpSequence<GSObject>)
        {
            ErrorHandler.AddError(new DefaultError("Match statements can only take a sequence as value", "semantic"));
            return;
        }
        GSharpSequence<GSObject> sequence = (GSharpSequence<GSObject>)value;
        int index = 0;
        foreach (GSObject obj in sequence.Sequence)
        {
            if (index == constantNames.Count - 1)
            {
                environment.SetVariable(constantNames[index], sequence.GetTail(index));
                break;
            }
            environment.SetVariable(constantNames[index], obj);
            index++;
        }
        if (index < constantNames.Count - 1)
        {
            for (int i = index; index < constantNames.Count; i++)
                environment.SetVariable(constantNames[index], null);
        }
    }
    public void VisitExpressionStatement(ExpressionStatement expression) => expression.Accept(this);
    public void VisitFunctionDeclaration(FunctionDeclaration declaration)
    {
        DeclaredFunction function = new(declaration);
        environment.SetFunction(function.Declaration.Name, function);
    }
    public GSObject VisitIfThenElse(IfThenElse ifThen)
    {
        bool condition = ifThen.Condition.Accept(this)?.ToValueOfTruth() == 1;
        if (condition)
            return ifThen.IfExpression.Accept(this);
        return ifThen.ElseExpression.Accept(this);
    }
    public GSObject VisitLetIn(LetIn letIn)
    {
        Context<GSObject, DeclaredFunction> letInContext = new(environment);
        environment = letInContext;
        foreach (var statement in letIn.Instructions)
            statement.Accept(this);
        GSObject result = letIn.Body.Accept(this);
        environment = environment.Enclosing;
        return result;
    }
    public GSObject VisitLiteralNumber(LiteralNumber literal) => literal.Value;
    public GSObject VisitUnaryOperation(UnaryOperation unary)
    {
        GSObject arg = unary.Argument.Accept(this);
        return arg.GetType() == unary.AcceptedType
            ? unary.Operation(arg)
            : throw new SemanticError($"Operator `{unary.OperationToken}`", unary.EnteredType.ToString(), arg.GetType().Name, null); ;
    }
    public GSObject VisitLiteralSequence(LiteralSequence sequence)
    {
        GSharpSequence<GSObject> result = sequence.GetSequenceValue(this);
        return result;
    }
    public GSObject VisitLiteralString(LiteralString @string) => @string.String;
    public void VisitDrawStatment(DrawStatement drawStatement)
    {
        var objectToDraw = drawStatement.Expression.Accept(this);
        var expressionType = drawStatement.Expression.ExpressionType;
        var label = drawStatement.StringExpression?.String;
        if (objectToDraw is IDrawable)
        {
            ((IDrawable)objectToDraw).Draw(Drawer, label);
            return;
        }
        else if (expressionType.Name == GTypeNames.GSequence && expressionType.IsFigure)
        {
            switch (expressionType.GenericType) 
            {
                case GTypeNames.Point: 
                    var pointSequence = (GSharpSequence<GSPoint>)objectToDraw;
                    Drawer.DrawSequence(pointSequence);
                    break;
                case GTypeNames.Line:
                    var lineSequence = (GSharpSequence<Line>)objectToDraw;
                    Drawer.DrawSequence(lineSequence);
                    break;
                case GTypeNames.Segment:
                    var segmentSequence = (GSharpSequence<Segment>)objectToDraw;
                    Drawer.DrawSequence(segmentSequence);
                    break;
                case GTypeNames.Ray:
                    var raySequence = (GSharpSequence<Ray>)objectToDraw;
                    Drawer.DrawSequence(raySequence);
                    break;
                case GTypeNames.Circle:
                    var circleSequence = (GSharpSequence<Circle>)objectToDraw;
                    Drawer.DrawSequence(circleSequence);
                    break;
                case GTypeNames.Arc:
                    var arcSequence = (GSharpSequence<Arc>)objectToDraw;
                    Drawer.DrawSequence(arcSequence);
                    break;
            }
            return;
        }
        throw new DefaultError("Draw argument must be a figure", "Semantic");
    }
    public void VisitColorStatent(ColorStatement color) => Drawer.SetColor(color.Color);
    public void VisitRestoreStatement(Restore restore) => Drawer.ResetColor();
    public void VisitRecieverStatement(Reciever reciever)
    {
        int coordinatesLimit = (Drawer.CanvasHeight + Drawer.CanvasWidth) / 2;
        if (reciever.IsSequence)
        {
            switch (reciever.ParameterType)
            {
                case GTypeNames.Point:
                    environment.SetVariable(reciever.Identifier, ArraySequence<GSPoint>.GetRandomInstance(coordinatesLimit));
                    break;
                case GTypeNames.Circle:
                    environment.SetVariable(reciever.Identifier, ArraySequence<Circle>.GetRandomInstance(coordinatesLimit));
                    break;
                case GTypeNames.Line:
                    environment.SetVariable(reciever.Identifier, ArraySequence<Line>.GetRandomInstance(coordinatesLimit));
                    break;
                case GTypeNames.Segment:
                    environment.SetVariable(reciever.Identifier, ArraySequence<Segment>.GetRandomInstance(coordinatesLimit));
                    break;
                case GTypeNames.Ray:
                    environment.SetVariable(reciever.Identifier, ArraySequence<Ray>.GetRandomInstance(coordinatesLimit));
                    break;
                case GTypeNames.Arc:
                    environment.SetVariable(reciever.Identifier, ArraySequence<Arc>.GetRandomInstance(coordinatesLimit));
                    break;
                default:
                    throw new DefaultError("Invalid type", "semantic");
            }
            return;
        }
        switch (reciever.ParameterType)
        {
            case GTypeNames.Point:
                environment.SetVariable(reciever.Identifier, GSPoint.GetRandomInstance(coordinatesLimit));
                break;
            case GTypeNames.Circle:
                environment.SetVariable(reciever.Identifier, Circle.GetRandomInstance(coordinatesLimit));
                break;
            case GTypeNames.Line:
                environment.SetVariable(reciever.Identifier, Line.GetRandomInstance(coordinatesLimit));
                break;
            case GTypeNames.Segment:
                environment.SetVariable(reciever.Identifier, Segment.GetRandomInstance(coordinatesLimit));
                break;
            case GTypeNames.Ray:
                environment.SetVariable(reciever.Identifier, Ray.GetRandomInstance(coordinatesLimit));
                break;
            case GTypeNames.Arc:
                environment.SetVariable(reciever.Identifier, Arc.GetRandomInstance(coordinatesLimit));
                break;
            default:
                throw new DefaultError("Invalid type", "semantic");
        }
    }
    public void VisitStatements(List<Statement> statements)
    {
        foreach(Statement st in statements)
            st.Accept(this);
    }
}