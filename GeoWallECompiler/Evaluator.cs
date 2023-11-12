using System.Xml.Linq;

namespace GeoWallECompiler;
public class Evaluator : IExpresionVisitor<GSharpObject>, IStatementVisitor<GSharpObject>
{
    /// <summary>
    /// Evalua la expresion binaria, luego de chequear que los tipos de entrada sean correctos
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Resultado de evaluar la operacion binaria</returns>
    /// <exception cref="SemanticError"></exception>
    public GSharpObject visitBinaryOperation(BinaryOperation binary)
    {
        GSharpObject left = binary.LeftArgument.Accept(this);
        GSharpObject right = binary.RightArgument.Accept(this);
        if (left.GetType() == binary.AcceptedType && right.GetType() == binary.AcceptedType)
            return binary.Operation(left, right);
        var conflictiveType = left.GetType() != binary.AcceptedType ? left.GetType().Name : right.GetType().Name;
        throw new SemanticError($"Operator `{binary.OperationToken}`", binary.ReturnedType.ToString(), conflictiveType);
    }
    public GSharpObject visitConstant(Constant constant) => constant.ValueExpression?.Accept(this);
    public GSharpObject visitConstantDeclaration(ConstantsDeclaration declaration) => null;
    public GSharpObject visitExpressionStatement(GSharpExpression expression) => null;
    public GSharpObject visitFunctionCall(FunctionCall functionCall) => throw new NotImplementedException();
    public GSharpObject visitIfThenElse(IfThenElse ifThen)
    {
        bool condition = ifThen.Condition.Accept(this)?.ToValueOfTruth() == 1;
        if (condition)
            return ifThen.IfExpression.Accept(this);
        return ifThen.ElseExpression.Accept(this);
    }
    public GSharpObject visitLetIn(LetIn letIn) => throw new NotImplementedException();
    public GSharpObject visitLiteral(Literal literal) => literal.Value;
    public GSharpObject visitUnaryOperation(UnaryOperation unary)
    {
        GSharpObject arg = unary.Argument.Accept(this);
        return arg.GetType() == unary.AcceptedType
            ? unary.Operation(arg)
            : throw new SemanticError($"Operator `{unary.OperationToken}`", unary.EnteredType.ToString(), arg.GetType().Name, null); ;
    }
}
