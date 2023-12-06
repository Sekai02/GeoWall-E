using GeoWallECompiler.Expressions;

namespace GeoWallECompiler.Visitors;
public class Resolver : IStatementVisitor, IExpressionVisitor<GSObject>
{
    Evaluator Interpreter;
    private Stack<Scope> Scopes { get; set; }
    public Resolver(Evaluator evaluator)
    {
        Scopes = new();
        BeginScope(); // global scope
        GSharp.InitializeGSharpStandard(Scopes.Peek());
        Interpreter = evaluator;
    }
    private void BeginScope() => Scopes.Push(new Scope());
    private void EndScope() => Scopes.Pop();
    private void DeclareVariable(string variableName)
    {
        if (variableName == "_")
            return;
        if (Scopes.Count == 0)
            return;
        Scope currentScope = Scopes.Peek();
        if (!currentScope.Variables.TryAdd(variableName, false))
            ErrorHandler.AddError(new DefaultError($"Variable {variableName} already exist in this scope", "Semantic"));
    }
    private void DefineVariable(string variableName)
    {
        if (Scopes.Count == 0)
            return;
        Scope currentScope = Scopes.Peek();
        currentScope.Variables[variableName] = true;
    }
    private void DeclareFunction(string functionName)
    {
        if (Scopes.Count == 0)
            return;
        Scope currentScope = Scopes.Peek();
        if (!currentScope.Functions.TryAdd(functionName, false))
            ErrorHandler.AddError(new DefaultError($"Function {functionName} already exist in this scope", "Semantic"));
    }
    private void DefineFunction(string functionName)
    {
        if (Scopes.Count == 0)
            return;
        Scope currentScope = Scopes.Peek();
        currentScope.Functions[functionName] = true;
    }
    private void BindValue(GSharpExpression expr, string variableName)
    {
        for (int i = 0; i < Scopes.Count; i++)
        {
            if (Scopes.ElementAt(i).Variables.ContainsKey(variableName))
            {
                Interpreter.ResolveReference(expr, i);
                return;
            }
        }
        ErrorHandler.AddError(new DefaultError($"Variable {variableName} is not defined", "Semantic"));
    }
    private void BindFunctionbody(GSharpExpression expr, string functionName)
    {
        for (int i = 0; i < Scopes.Count; i++)
        {
            if (Scopes.ElementAt(i).Functions.ContainsKey(functionName))
            {
                Interpreter.ResolveReference(expr, i);
                return;
            }
        }
        ErrorHandler.AddError(new DefaultError($"Function {functionName} is not defined", "Semantic"));
    }
    public GSObject VisitBinaryOperation(BinaryOperation binary)
    {
        binary.LeftArgument.Accept(this);
        binary.RightArgument.Accept(this);
        return null;
    }
    public GSObject VisitConstant(Constant constant)
    {
        //if (Scopes.Count == 0)
        //    ErrorHandler.AddError(new DefaultError("Local variable is not defined", "semantic"));
        //else
        //{
        //}

        var scope = Scopes.Peek();
        if (scope.Variables.ContainsKey(constant.Name))
        {
            if (scope.Variables[constant.Name] == false)
            {
                ErrorHandler.AddError(new DefaultError("Can't read local variable in its own initializer", "semantic"));
                return null;
            }
        }
        BindValue(constant, constant.Name);
        return null;
    }
    public void VisitConstantDeclaration(ConstantsDeclaration declaration)
    {
        foreach (string constant in declaration.ConstantNames)
            DeclareVariable(constant);
        declaration.ValueExpression.Accept(this);
        foreach (string constant in declaration.ConstantNames)
            DefineVariable(constant);
    }
    public void VisitExpressionStatement(ExpressionStatement expression) => expression.Expression.Accept(this);
    public GSObject VisitFunctionCall(FunctionCall functionCall)
    {
        foreach (GSharpExpression argument in functionCall.Arguments)
            argument.Accept(this);
        BindFunctionbody(functionCall, functionCall.FunctionName);
        return null;
    }
    public void VisitFunctionDeclaration(FunctionDeclaration declaration)
    {
        DeclareFunction(declaration.Name);
        DefineFunction(declaration.Name);
        BeginScope();
        foreach (string parameter in declaration.Parameters)
        {
            DeclareVariable(parameter);
            DefineVariable(parameter);
        }
        declaration.Body.Accept(this);
        EndScope();
    }
    public GSObject VisitIfThenElse(IfThenElse ifThen)
    {
        ifThen.Condition.Accept(this);
        ifThen.IfExpression.Accept(this);
        ifThen.ElseExpression.Accept(this);
        return null;
    }
    public GSObject VisitLetIn(LetIn letIn)
    {
        BeginScope();
        foreach (Statement instruction in letIn.Instructions)
            instruction.Accept(this);
        letIn.Body.Accept(this);
        EndScope();
        return null;
    }
    public GSObject VisitLiteralNumber(LiteralNumber literal) => null;
    public GSObject VisitUnaryOperation(UnaryOperation unary)
    {
        unary.Argument.Accept(this);
        return null;
    }
    public GSObject VisitLiteralSequence(LiteralSequence sequence)
    {
        if (sequence is LiteralArrayExpression)
        {
            foreach (var expression in sequence.Expressions)
                expression.Accept(this);
        }
        return null;
    }
    public void VisitDrawStatement(DrawStatement drawStatement) => drawStatement.Expression.Accept(this);
    public void VisitColorStatent(ColorStatement color) { return; }
    public void VisitRestoreStatement(Restore restore) { return; }
    public GSObject VisitLiteralString(LiteralString @string) => null;
    public void VisitStatements(List<Statement> statements)
    {
        foreach (Statement st in statements)
        {
            try
            {
                st.Accept(this);
            }
            catch (Exception ex)
            {
                ErrorHandler.AddError(new DefaultError(ex.Message));
            }
        }
    }
    public void VisitRecieverStatement(Reciever reciever)
    {
        DeclareVariable(reciever.Identifier);
        DefineVariable(reciever.Identifier);
    }
    public void VisitPrintStatement(PrintStatement printStatement) => printStatement.Expression.Accept(this);
    public GSObject VisitLiteralUndefined(LiteralUndefined undefined) => null;
}
