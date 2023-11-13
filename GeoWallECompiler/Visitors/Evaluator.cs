namespace GeoWallECompiler;
public class Evaluator : IExpressionVisitor<GSharpObject>, IStatementVisitor
{
    private Context environment = new();
    /// <summary>
    /// Evalua la expresion binaria, luego de chequear que los tipos de entrada sean correctos
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Resultado de evaluar la operacion binaria</returns>
    /// <exception cref="SemanticError"></exception>
    public GSharpObject VisitBinaryOperation(BinaryOperation binary)
    {
        GSharpObject left = binary.LeftArgument.Accept(this);
        GSharpObject right = binary.RightArgument.Accept(this);
        if (left.GetType() == binary.AcceptedType && right.GetType() == binary.AcceptedType)
            return binary.Operation(left, right);
        var conflictiveType = left.GetType() != binary.AcceptedType ? left.GetType().Name : right.GetType().Name;
        throw new SemanticError($"Operator `{binary.OperationToken}`", binary.ReturnedType.ToString(), conflictiveType);
    }
    public GSharpObject VisitConstant(Constant constant) => environment.GetVariableValue(constant.Name);
    public GSharpObject VisitFunctionCall(FunctionCall functionCall) => throw new NotImplementedException();
    public void VisitConstantDeclaration(ConstantsDeclaration declaration)
    {
        GSharpObject value = declaration.Value.Accept(new Evaluator());
        List<string> constantNames = declaration.ConstantNames;
        if (value == null) // error
            ;
        if (constantNames.Count == 1)
        {
            environment.DefineVariable(constantNames[0], value);
            return;
        }
        if (value is not GSharpSequence<GSharpObject>)
            throw new SemanticError("Match declaration", "squence", "otra cosa"); //arreglar
        GSharpSequence<GSharpObject> sequence = value as GSharpSequence<GSharpObject>;
        int index = 0;
        foreach (GSharpObject obj in sequence.Sequence)
        {
            if (index == constantNames.Count - 1)
            {
                environment.DefineVariable(constantNames[index], obj);
                break;
            }
            environment.DefineVariable(constantNames[index], obj);
            index++;
        }
        if (index < constantNames.Count - 1)
        {
            for (int i = index; index < constantNames.Count; i++)
                environment.DefineVariable(constantNames[index], null);
        }

    }
    public void VisitExpressionStatement(ExpressionStatement expression) => expression.Accept(this);
    public GSharpObject VisitIfThenElse(IfThenElse ifThen)
    {
        bool condition = ifThen.Condition.Accept(this)?.ToValueOfTruth() == 1;
        if (condition)
            return ifThen.IfExpression.Accept(this);
        return ifThen.ElseExpression.Accept(this);
    }
    public GSharpObject VisitLetIn(LetIn letIn) 
    {
        Context letInContext = new(environment);
        environment = letInContext;
        foreach(var declaration in letIn.DeclaredConstants)
            declaration.Accept(this);
        GSharpObject result = letIn.Body.Accept(this);
        environment = environment.Enclosing;
        return result;
    }
    public GSharpObject VisitLiteral(Literal literal) => literal.Value;
    public GSharpObject VisitUnaryOperation(UnaryOperation unary)
    {
        GSharpObject arg = unary.Argument.Accept(this);
        return arg.GetType() == unary.AcceptedType
            ? unary.Operation(arg)
            : throw new SemanticError($"Operator `{unary.OperationToken}`", unary.EnteredType.ToString(), arg.GetType().Name, null); ;
    }
}
