using GeoWallECompiler.Expressions;
using System;
using System.Globalization;
using System.Reflection.Metadata;

namespace GeoWallECompiler;
public class TypeChecker : IExpressionVisitor<GSharpType>, IStatementVisitor
{
    Evaluator Interpreter;
    public Context<GSharpType, ICallable> TypeEnvironment;
    private bool IsCheckingFunctionCall = false;
    private readonly int MaximunStackCalls = 1000;
    private int currentStackFrame = 0;
    public TypeChecker(Evaluator evaluator)
    {
        Interpreter = evaluator;
        TypeEnvironment = new();
        GSharp.InitializeGSharpStandard(TypeEnvironment);
        IsCheckingFunctionCall = false;
    }
    public GSharpType VisitBinaryOperation(BinaryOperation binary)
    {
        var leftType = binary.LeftArgument.Accept(this);
        var rightType = binary.RightArgument.Accept(this);

        binary.LeftArgument.ExpressionType = leftType;
        binary.RightArgument.ExpressionType = rightType;
        
        foreach(BinaryOverloadInfo overload in binary.PosibleOverloads)
        {
            if (BinaryOperation.IsAnAcceptedOverload(leftType, rightType, overload))
                return overload.GetType(leftType, rightType);
        }
        ReportSemanticError(new DefaultError($"Operator '{binary.OperationToken}' cannot be used between {leftType} and {rightType}"));
        return binary.PosibleOverloads[0].ReturnedType;
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
        if (!type.HasGenericType && type.Name != GTypeNames.Undetermined)
        {
            ReportSemanticError(new DefaultError("Match statements can only take a sequence as value", "semantic"));
            return;
        }
        for (int i = 0; i < declaration.ConstantNames.Count - 1; i++)
        {
            string? constant = declaration.ConstantNames[i];
            TypeEnvironment.SetVariable(constant, new(type.GenericType));
        }
        string? lastConstant = declaration.ConstantNames[^1];
        TypeEnvironment.SetVariable(lastConstant, type);
    }
    public void VisitDrawStatement(DrawStatement drawStatement) 
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
        currentStackFrame++;
        if (currentStackFrame > MaximunStackCalls)
            throw new OverFlowError(functionCall.FunctionName);

        int distance = Interpreter.References[functionCall];
        ICallable callee = TypeEnvironment.AccessFunctionAt(distance, functionCall.FunctionName);
        List<GSharpType> arguments = new();
        foreach (GSharpExpression argument in functionCall.Arguments)
        {
            GSharpType argumentType = argument.Accept(this);
            argument.ExpressionType = argument.ExpressionType;
            arguments.Add(argumentType);
        }
        IsCheckingFunctionCall = true;
        var result = callee.GetType(this, arguments);
        IsCheckingFunctionCall = false;
        currentStackFrame--;
        return result;
    }
    public void VisitFunctionDeclaration(FunctionDeclaration declaration)
    {
        DeclaredFunction function = new(declaration);
        TypeEnvironment.SetFunction(function.Declaration.Name, function);
        function.SetType(this);
    }
    public GSharpType VisitIfThenElse(IfThenElse ifThen)
    {
        ifThen.Condition.ExpressionType = ifThen.Condition.Accept(this);
        GSharpType ifType = ifThen.IfExpression.Accept(this);
        GSharpType elseType = ifThen.ElseExpression.Accept(this);
        ifThen.IfExpression.ExpressionType = ifType;
        ifThen.ElseExpression.ExpressionType = elseType;

        ifThen.ExpressionType = ifType;
        if (ifType != elseType)
            ReportSemanticError(new DefaultError("SemanticError", "if-then-else statements must have the same return type"));
        return ifType;
    }
    public GSharpType VisitLetIn(LetIn letIn)
    {
        Context<GSharpType, ICallable> letInContext = new(TypeEnvironment);
        TypeEnvironment = letInContext;
        foreach (Statement instruction in letIn.Instructions)
            instruction.Accept(this);
        var result = letIn.Body.Accept(this);
        TypeEnvironment = TypeEnvironment.Enclosing!;
        return result;
    }
    public GSharpType VisitLiteralNumber(LiteralNumber literal) => literal.ExpressionType = new(GTypeNames.GNumber);
    public GSharpType VisitLiteralSequence(LiteralSequence sequence)
    {
        if (sequence.TypeSetted)
            return sequence.ExpressionType;
        foreach (GSharpExpression expression in sequence.Expressions)
        {
            GSharpType expressionType = expression.Accept(this);
            if (!sequence.TypeSetted && expressionType.Name != GTypeNames.Undetermined)
                sequence.ExpressionType = new(GTypeNames.GSequence, expressionType.Name);
            else if (sequence.ExpressionType.GenericType != expressionType.Name)
            {
                ReportSemanticError(new DefaultError("All elements of a sequence must be of the same type", "SemanticError"));
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

        foreach (UnaryOverloadInfo overload in unary.PosibleOverloads)
        {
            if (UnaryOperation.IsAnAcceptedOverload(argType, overload))
                return overload.ReturnedType;
        }
        ReportSemanticError(new DefaultError($"Operator '{unary.OperationToken}' cannot be used with {argType}"));
        return unary.PosibleOverloads[0].ReturnedType;
    }
    public void VisitStatements(List<Statement> statements)
    {
        foreach (Statement st in statements)
        {
            try
            {
                st.Accept(this);
            }
            catch(Exception ex)
            {
                ErrorHandler.AddError(new DefaultError(ex.Message));
            }
        }
    }
    private void ReportSemanticError(GSharpException error)
    {
        if (IsCheckingFunctionCall)
            throw error;
        ErrorHandler.AddError(error);
    }
    public void VisitPrintStatement(PrintStatement printStatement) 
    {
        GSharpType type = printStatement.Expression.Accept(this);
        printStatement.Expression.ExpressionType = type;
    }
    public GSharpType VisitLiteralUndefined(LiteralUndefined undefined) => new(GTypeNames.GObject);
    public void VisitImportStatement(Import import)
    {
        Context<GSharpType, ICallable> context = import.FetchedProgram.TypeEnvironment;
        TypeEnvironment.EatContext(context);
    }
}