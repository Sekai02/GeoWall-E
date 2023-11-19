namespace GeoWallECompiler;

public interface IExpressionVisitor<T>
{
    T VisitLetIn(LetIn letIn);
    T VisitIfThenElse(IfThenElse ifThen);
    T VisitBinaryOperation(BinaryOperation binary);
    T VisitUnaryOperation(UnaryOperation unary);
    T VisitLiteralNumber(LiteralNumber literal);
    T VisitLiteralString(LiteralString @string);
    T VisitConstant(Constant constant);
    T VisitFunctionCall(FunctionCall functionCall);
    T VisitLiteralSequence(LiteralSequence sequence);
}
