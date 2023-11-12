namespace GeoWallECompiler;
public interface IStatementVisitor<T>
{
    T visitExpressionStatement(GSharpExpression expression);
    //T visitFunctionDeclaration(FunctionDeclaration declaration);
    T visitConstantDeclaration(ConstantsDeclaration declaration);
}
public interface IExpresionVisitor<T> : IStatementVisitor<T>
{
    T visitLetIn(LetIn letIn);
    T visitIfThenElse(IfThenElse ifThen);
    T visitBinaryOperation(BinaryOperation binary);
    T visitUnaryOperation(UnaryOperation unary);
    T visitLiteral(Literal literal);
    T visitConstant(Constant constant);
    T visitFunctionCall(FunctionCall functionCall);
}
