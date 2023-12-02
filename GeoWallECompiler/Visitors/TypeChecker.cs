using GeoWallECompiler.Expressions;
using System.Reflection.Metadata;

namespace GeoWallECompiler;
public class TypeChecker : IExpressionVisitor<GSharpType>, IStatementVisitor
{
    Evaluator Interpreter;
    public Context<GSharpType, DeclaredFunction> TypeEnvironment;
    private bool IsCheckingFunctionCall = false;
    public TypeChecker(Evaluator evaluator)
    {
        Interpreter = evaluator;
        TypeEnvironment = new();
        IsCheckingFunctionCall = false;
    }
    public GSharpType VisitBinaryOperation(BinaryOperation binary)
    {
        if (binary.EnteredType.Name == GTypeNames.Undetermined)
            return binary.ReturnedType;
        var leftType = binary.LeftArgument.Accept(this);
        var rightType = binary.RightArgument.Accept(this);

        binary.LeftArgument.ExpressionType = leftType;
        binary.RightArgument.ExpressionType = rightType;
        
        if (leftType != binary.EnteredType && leftType.Name != GTypeNames.Undetermined)
            ReportSemanticError(new SemanticError($"Operator `{binary.OperationToken}`", binary.EnteredType.ToString(), leftType.ToString()));
        if (rightType != binary.EnteredType && rightType.Name != GTypeNames.Undetermined)
            ReportSemanticError(new SemanticError($"Operator `{binary.OperationToken}`", binary.EnteredType.ToString(), rightType.ToString()));
        
        binary.ExpressionType = binary.ReturnedType;
        return binary.ReturnedType;
    }
    public void VisitColorStatent(ColorStatement color) { return; }
    public GSharpType VisitConstant(Constant constant)
    {
        int distance = Interpreter.References[constant];
        GSharpType result = TypeEnvironment.AccessVariableAt(distance, constant.Name);
        constant.ExpressionType = result;
        return result;
    }
    public void VisitConstantDeclaration(ConstantsDeclaration declaration)
    {
        GSharpType type = declaration.ValueExpression.Accept(this);
        declaration.ValueExpression.ExpressionType = type;
        if(declaration.ConstantNames.Count == 1)
        {
            TypeEnvironment.SetVariable(declaration.ConstantNames[0], type);
            return;
        }
        if (!type.HasGenericType)
        {
            ReportSemanticError(new DefaultError("Match statements can only take a sequence as value", "semantic"));
            return;
        }
        foreach (var constant in declaration.ConstantNames)
            TypeEnvironment.SetVariable(constant, new(type.GenericType));     
    }
    public void VisitDrawStatment(DrawStatement drawStatement) 
    {
        var type = drawStatement.Expression.Accept(this);
        drawStatement.Expression.ExpressionType = type;
    }
    public void VisitExpressionStatement(ExpressionStatement expression)
    {
        GSharpType gSharpType = expression.Expression.Accept(this);
        expression.Expression.ExpressionType = gSharpType;
    }
    public GSharpType VisitFunctionCall(FunctionCall functionCall)
    {
        int distance = Interpreter.References[functionCall];
        DeclaredFunction callee = TypeEnvironment.AccessFunctionAt(distance, functionCall.FunctionName);
        List<GSharpType> arguments = new();
        foreach (GSharpExpression argument in functionCall.Arguments)
        {
            GSharpType argumentType = argument.Accept(this);
            argument.ExpressionType = argument.ExpressionType;
            arguments.Add(argumentType);
        }
        return callee.GetType(this, arguments);
    }
    public void VisitFunctionDeclaration(FunctionDeclaration declaration)
    {
        DeclaredFunction function = new(declaration);
        TypeEnvironment.SetFunction(function.Declaration.Name, function);
    }
    public GSharpType VisitIfThenElse(IfThenElse ifThen)
    {
        ifThen.Condition.ExpressionType = ifThen.Condition.Accept(this);
        GSharpType ifType = ifThen.IfExpression.Accept(this);
        GSharpType elseType = ifThen.ElseExpression.Accept(this);
        ifThen.IfExpression.ExpressionType = ifType;
        ifThen.ElseExpression.ExpressionType = elseType;
        if (ifType == elseType)
        {
            ifThen.ExpressionType = ifType;
            return ifType;
        }
        else
        {
            ReportSemanticError(new DefaultError("SemanticError", "if-then-else statements must have the same return type"));
            return new(GTypeNames.Undetermined);
        }
    }
    public GSharpType VisitLetIn(LetIn letIn)
    {
        Context<GSharpType, DeclaredFunction> letInContext = new(TypeEnvironment);
        TypeEnvironment = letInContext;
        foreach (Statement instruction in letIn.Instructions)
            instruction.Accept(this);
        var result = letIn.Body.Accept(this);
        TypeEnvironment = TypeEnvironment.Enclosing;
        return result;
    }
    public GSharpType VisitLiteralNumber(LiteralNumber literal) => literal.ExpressionType = new(GTypeNames.GNumber);
    public GSharpType VisitLiteralSequence(LiteralSequence sequence)
    {
        //Arreglar
        foreach (GSharpExpression expression in sequence.Expressions)
        {
            GSharpType expressionType = expression.Accept(this);
            if (!sequence.TypeSetted)
                sequence.ExpressionType = new(GTypeNames.GSequence, expressionType.Name);
            else if (sequence.ExpressionType.GenericType != expressionType.Name)
            {
                ReportSemanticError(new DefaultError("SemanticError", "All elements of a sequence must be of the same type"));
                return sequence.ExpressionType;
            }
        }
        return sequence.ExpressionType;
    }
    public GSharpType VisitLiteralString(LiteralString @string) => @string.ExpressionType = new(GTypeNames.GString);
    public void VisitRestoreStatement(Restore restore) { return; }
    public void VisitRecieverStatement(Reciever reciever) 
    {
        GSharpType recievedVarType = reciever.IsSequence ? 
            new(GTypeNames.GSequence, reciever.ParameterType) 
            : new(reciever.ParameterType);
        TypeEnvironment.SetVariable(reciever.Identifier, recievedVarType);
    }
    public GSharpType VisitUnaryOperation(UnaryOperation unary)
    {
        GSharpType argType = unary.Argument.Accept(this);
        unary.ExpressionType = argType;
        if (argType.Name != GTypeNames.Undetermined && argType != unary.EnteredType)
            ReportSemanticError(new SemanticError($"Operator `{unary.OperationToken}`", unary.EnteredType.Name.ToString(), argType.Name.ToString(), null));
        return unary.ReturnedType;
    }
    public void VisitStatements(List<Statement> statements)
    {
        foreach (Statement st in statements)
            st.Accept(this);
    }
    private void ReportSemanticError(GSharpException error)
    {
        if (IsCheckingFunctionCall)
            throw error;
        ErrorHandler.AddError(error);
    }
}
