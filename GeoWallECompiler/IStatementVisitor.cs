namespace GeoWallECompiler;
public interface IStatementVisitor
{
    void VisitExpressionStatement(ExpressionStatement expression);
    void VisitFunctionDeclaration(FunctionDeclaration declaration);
    void VisitConstantDeclaration(ConstantsDeclaration declaration);
}
public interface IExpressionVisitor<T>
{
    T VisitLetIn(LetIn letIn);
    T VisitIfThenElse(IfThenElse ifThen);
    T VisitBinaryOperation(BinaryOperation binary);
    T VisitUnaryOperation(UnaryOperation unary);
    T VisitLiteral(LiteralNumber literal);
    T VisitConstant(Constant constant);
    T VisitFunctionCall(FunctionCall functionCall);
    T VisitLiteralSequence(LiteralSequence sequence);
}
