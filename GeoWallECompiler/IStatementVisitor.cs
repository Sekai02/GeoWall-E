namespace GeoWallECompiler;
public interface IStatementVisitor
{
    void visitExpressionStatement(GSharpExpression expression);
    //T visitFunctionDeclaration(FunctionDeclaration declaration);
    void visitConstantDeclaration(ConstantsDeclaration declaration);
}
public interface IExpressionVisitor<T>
{
    T visitLetIn(LetIn letIn);
    T visitIfThenElse(IfThenElse ifThen);
    T visitBinaryOperation(BinaryOperation binary);
    T visitUnaryOperation(UnaryOperation unary);
    T visitLiteral(Literal literal);
    T visitConstant(Constant constant);
    T visitFunctionCall(FunctionCall functionCall);
}
