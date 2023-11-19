namespace GeoWallECompiler;
public class TypeChecker : IExpressionVisitor<GSharpTypes>
{
    public GSharpTypes VisitBinaryOperation(BinaryOperation binary) 
    {
        if (binary.EnteredType == GSharpTypes.Undetermined)
            return binary.ReturnedType;
        var leftType = binary.LeftArgument.Accept(this);
        var rightType = binary.RightArgument.Accept(this);
        if (leftType != binary.EnteredType && leftType != GSharpTypes.Undetermined)
            ErrorHandler.AddError(new SemanticError($"Operator `{binary.OperationToken}`", binary.EnteredType.ToString(), leftType.ToString()));
        if (rightType != binary.EnteredType && rightType != GSharpTypes.Undetermined)
            ErrorHandler.AddError(new SemanticError($"Operator `{binary.OperationToken}`", binary.EnteredType.ToString(), rightType.ToString()));
        return binary.ReturnedType;
    }
    public GSharpTypes VisitConstant(Constant constant) => constant.Accept(this);
    public GSharpTypes VisitFunctionCall(FunctionCall functionCall) => GSharpTypes.Undetermined;
    public GSharpTypes VisitIfThenElse(IfThenElse ifThen)
    {
        ifThen.Condition.Accept(this);
        GSharpTypes ifType = ifThen.IfExpression.Accept(this);
        GSharpTypes elseType = ifThen.ElseExpression.Accept(this);
        return ifType == elseType ? ifType : throw new DefaultError("SemanticError", "if-then-else statements must have the same return type");
    }
    public GSharpTypes VisitLetIn(LetIn letIn) 
    {
        //foreach (Constant constant in letIn.DeclaredConstants)
        //    constant.Accept(this);
        return letIn.Body.Accept(this);
    }
    public GSharpTypes VisitLiteral(LiteralNumber literal) => literal.Type;
    public GSharpTypes VisitLiteralSequence(LiteralSequence sequence) => throw new NotImplementedException();

    public GSharpTypes VisitUnaryOperation(UnaryOperation unary)
    {
        GSharpTypes argType = unary.Argument.Accept(this);
        return argType != GSharpTypes.Undetermined && argType != unary.EnteredType
            ? throw new SemanticError($"Operator `{unary.OperationToken}`", unary.EnteredType.ToString(), argType.ToString(), null)
            : unary.ReturnedType;
    }
}
