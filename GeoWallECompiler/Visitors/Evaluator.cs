using GeoWallECompiler.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace GeoWallECompiler;
public class Evaluator : IExpressionVisitor<GSharpObject>, IStatementVisitor
{
    public Dictionary<GSharpExpression, int> References = new();
    private Context<GSharpObject, DeclaredFunction>? environment = new();
    public Evaluator(IDrawer drawer)
    {
        environment = new();
        Drawer = drawer;
    }
    public Context<GSharpObject, DeclaredFunction>? EvaluationContext { get => environment; set => environment = value; }
    public IDrawer Drawer { get; }
    public void ResolveReference(GSharpExpression expression, int depth)
    {
        References.Add(expression, depth);
    }
    public GSharpObject VisitBinaryOperation(BinaryOperation binary)
    {
        GSharpObject left = binary.LeftArgument.Accept(this);
        GSharpObject right = binary.RightArgument.Accept(this);
        if (left.GetType() == binary.AcceptedType && right.GetType() == binary.AcceptedType)
            return binary.Operation(left, right);
        var conflictiveType = left.GetType() != binary.AcceptedType ? left.GetType().Name : right.GetType().Name;
        throw new SemanticError($"Operator `{binary.OperationToken}`", binary.ReturnedType.ToString(), conflictiveType);
    }
    public GSharpObject VisitConstant(Constant constant)
    {
        int distance = References[constant];
        return environment.AccessVariableAt(distance, constant.Name);
    }
    public GSharpObject VisitFunctionCall(FunctionCall functionCall)
    {
        int distance = References[functionCall];
        DeclaredFunction callee = environment.AccessFunctionAt(distance, functionCall.FunctionName);
        List<GSharpObject> arguments = new();
        foreach (GSharpExpression argument in functionCall.Arguments)
        {
            arguments.Add(argument.Accept(this));
        }
        return callee.Evaluate(this, arguments);
    }
    public void VisitConstantDeclaration(ConstantsDeclaration declaration)
    {
        GSharpObject value = declaration.Value.Accept(this);
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
        if (value is not GSharpSequence<GSharpObject>)
        {
            ErrorHandler.AddError(new DefaultError("Match statements can only take a sequence as value", "semantic"));
            return;
        }
        GSharpSequence<GSharpObject> sequence = (GSharpSequence<GSharpObject>)value;
        int index = 0;
        foreach (GSharpObject obj in sequence.Sequence)
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
    public GSharpObject VisitIfThenElse(IfThenElse ifThen)
    {
        bool condition = ifThen.Condition.Accept(this)?.ToValueOfTruth() == 1;
        if (condition)
            return ifThen.IfExpression.Accept(this);
        return ifThen.ElseExpression.Accept(this);
    }
    public GSharpObject VisitLetIn(LetIn letIn)
    {
        Context<GSharpObject, DeclaredFunction> letInContext = new(environment);
        environment = letInContext;
        foreach (var declaration in letIn.DeclaredConstants)
            declaration.Accept(this);
        GSharpObject result = letIn.Body.Accept(this);
        environment = environment.Enclosing;
        return result;
    }
    public GSharpObject VisitLiteralNumber(LiteralNumber literal) => literal.Value;
    public GSharpObject VisitUnaryOperation(UnaryOperation unary)
    {
        GSharpObject arg = unary.Argument.Accept(this);
        return arg.GetType() == unary.AcceptedType
            ? unary.Operation(arg)
            : throw new SemanticError($"Operator `{unary.OperationToken}`", unary.EnteredType.ToString(), arg.GetType().Name, null); ;
    }
    public GSharpObject VisitLiteralSequence(LiteralSequence sequence)
    {
        GSharpSequence<GSharpObject> result = sequence.GetSequenceValue(this);
        return result;
    }
    public GSharpObject VisitLiteralString(LiteralString @string) => @string.String;
    public void VisitDrawStatment(DrawStatement drawStatement)
    {
        var objectToDraw = drawStatement.Expression.Accept(this);
        if (objectToDraw is not IDrawable)
            throw new DefaultError("Draw argument must be a figure", "Semantic");
    }
    public void VisitColorStatent(ColorStatement color) => Drawer.SetColor(color.Color);
    public void VisitRestoreStatement(Restore restore) => Drawer.ResetColor();
    public void VisitRecieverStatement(Reciever reciever)
    {
        if (reciever.IsSequence)
        {
            switch (reciever.ParameterType)
            {
                case GSharpTypes.GNumber:
                    environment.SetVariable(reciever.Identifier, ArraySequence<GSharpNumber>.GetRandomInstance());
                    break;
                case GSharpTypes.Point:
                    environment.SetVariable(reciever.Identifier, ArraySequence<GSharpPoint>.GetRandomInstance());
                    break;
                case GSharpTypes.Circle:
                    environment.SetVariable(reciever.Identifier, ArraySequence<Circle>.GetRandomInstance());    
                    break;
                case GSharpTypes.Line:
                    environment.SetVariable(reciever.Identifier, ArraySequence<Line>.GetRandomInstance());
                    break;
                case GSharpTypes.Ray:
                    environment.SetVariable(reciever.Identifier, ArraySequence<Ray>.GetRandomInstance());
                    break;
                case GSharpTypes.Arc:
                    environment.SetVariable(reciever.Identifier, ArraySequence<Arc>.GetRandomInstance());
                    break;
                default:
                    throw new DefaultError("Invalid type", "semantic");

            }
            return;
        }
        switch (reciever.ParameterType)
        {
            case GSharpTypes.GNumber:
                environment.SetVariable(reciever.Identifier, GSharpNumber.GetRandomInstance());
                break;
            case GSharpTypes.Point:
                environment.SetVariable(reciever.Identifier, GSharpPoint.GetRandomInstance());
                break;
            case GSharpTypes.Circle:
                environment.SetVariable(reciever.Identifier, Circle.GetRandomInstance());
                break;
            case GSharpTypes.Line:
                environment.SetVariable(reciever.Identifier, Line.GetRandomInstance());
                break;
            case GSharpTypes.Ray:
                environment.SetVariable(reciever.Identifier, Ray.GetRandomInstance());
                break;
            case GSharpTypes.Arc:
                environment.SetVariable(reciever.Identifier, Arc.GetRandomInstance());
                break;
            default:
                throw new DefaultError("Invalid type", "semantic");
        }
    }
}