using GeoWallECompiler.Expressions;
using System.Reflection.Metadata;

namespace GeoWallECompiler;
public class TypeChecker : IExpressionVisitor<GSharpTypes>, IStatementVisitor
{
    Evaluator Interpreter;
    public Context<GSharpTypes, DeclaredFunction> TypeEnvironment;
    private bool IsCheckingFunctionCall = false;
    public TypeChecker(Evaluator evaluator)
    {
        Interpreter = evaluator;
        TypeEnvironment = new();
        IsCheckingFunctionCall = false;
    }
    public GSharpTypes VisitBinaryOperation(BinaryOperation binary)
    {
        if (binary.EnteredType == GSharpTypes.Undetermined)
            return binary.ReturnedType;
        var leftType = binary.LeftArgument.Accept(this);
        var rightType = binary.RightArgument.Accept(this);
        if (leftType != binary.EnteredType && leftType != GSharpTypes.Undetermined)
            ReportSemanticError(new SemanticError($"Operator `{binary.OperationToken}`", binary.EnteredType.ToString(), leftType.ToString()));
        if (rightType != binary.EnteredType && rightType != GSharpTypes.Undetermined)
            ReportSemanticError(new SemanticError($"Operator `{binary.OperationToken}`", binary.EnteredType.ToString(), rightType.ToString()));
        return binary.ReturnedType;
    }
    public void VisitColorStatent(ColorStatement color) { return; }
    public GSharpTypes VisitConstant(Constant constant)
    {
        int distance = Interpreter.References[constant];
        return TypeEnvironment.AccessVariableAt(distance, constant.Name);
    }
    public void VisitConstantDeclaration(ConstantsDeclaration declaration)
    {
        GSharpTypes type = declaration.Value.Accept(this);
        if (declaration.ConstantNames.Count != 1 && !IsSequence(type))
        {
            ReportSemanticError(new DefaultError("Match statements can only take a sequence as value", "semantic"));
            return;
        }
        foreach (var constant in declaration.ConstantNames)
            TypeEnvironment.SetVariable(constant, type);     
    }
    public void VisitDrawStatment(DrawStatement drawStatement) => drawStatement.Expression.Accept(this);
    public void VisitExpressionStatement(ExpressionStatement expression) => expression.Expression.Accept(this);
    public GSharpTypes VisitFunctionCall(FunctionCall functionCall)
    {
        int distance = Interpreter.References[functionCall];
        DeclaredFunction callee = TypeEnvironment.AccessFunctionAt(distance, functionCall.FunctionName);
        List<GSharpTypes> arguments = new();
        foreach (GSharpExpression argument in functionCall.Arguments)
        {
            arguments.Add(argument.Accept(this));
        }
        return callee.GetType(this, arguments);
    }
    public void VisitFunctionDeclaration(FunctionDeclaration declaration)
    {
        DeclaredFunction function = new(declaration);
        TypeEnvironment.SetFunction(function.Declaration.Name, function);
    }
    public GSharpTypes VisitIfThenElse(IfThenElse ifThen)
    {
        ifThen.Condition.Accept(this);
        GSharpTypes ifType = ifThen.IfExpression.Accept(this);
        GSharpTypes elseType = ifThen.ElseExpression.Accept(this);
        if (ifType == elseType)
            return ifType;
        else
        {
            ReportSemanticError(new DefaultError("SemanticError", "if-then-else statements must have the same return type"));
            return GSharpTypes.Undetermined;
        }
    }
    public GSharpTypes VisitLetIn(LetIn letIn)
    {
        Context<GSharpTypes, DeclaredFunction> letInContext = new(TypeEnvironment);
        TypeEnvironment = letInContext;
        foreach (ConstantsDeclaration constant in letIn.DeclaredConstants)
            constant.Accept(this);
        var result = letIn.Body.Accept(this);
        TypeEnvironment = TypeEnvironment.Enclosing;
        return result;
    }
    public GSharpTypes VisitLiteralNumber(LiteralNumber literal) => GSharpTypes.GNumber;
    public GSharpTypes VisitLiteralSequence(LiteralSequence sequence)
    {
        GSharpTypes type = GSharpTypes.Undetermined;
        foreach (GSharpExpression expression in sequence.Expressions)
        {
            GSharpTypes expressionType = expression.Accept(this);
            if (type == GSharpTypes.Undetermined)
                type = expressionType;
            else if (type != expressionType)
            {
                ReportSemanticError(new DefaultError("SemanticError", "All elements of a sequence must be of the same type"));
                return type;
            }
        }
        return type;
    }
    public GSharpTypes VisitLiteralString(LiteralString @string) => GSharpTypes.GString;
    public void VisitRestoreStatement(Restore restore) { }
    public void VisitRecieverStatement(Reciever reciever) { }
    public GSharpTypes VisitUnaryOperation(UnaryOperation unary)
    {
        GSharpTypes argType = unary.Argument.Accept(this);
        if (argType != GSharpTypes.Undetermined && argType != unary.EnteredType)
            ReportSemanticError(new SemanticError($"Operator `{unary.OperationToken}`", unary.EnteredType.ToString(), argType.ToString(), null));
        return unary.ReturnedType;
    }
    private void ReportSemanticError(GSharpException error)
    {
        if (IsCheckingFunctionCall)
            throw error;
        ErrorHandler.AddError(error);
    }
    private bool IsSequence(GSharpTypes type)
    {
        return type is GSharpTypes.ArcSequence
            or GSharpTypes.RaySequence
            or GSharpTypes.LineSequence
            or GSharpTypes.CircleSequence
            or GSharpTypes.PointSequence
            or GSharpTypes.SegmentSequence
            or GSharpTypes.GSequence;       
    }
}
