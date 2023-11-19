using System.Reflection.Metadata.Ecma335;

namespace GeoWallECompiler.Visitors;
public class Resolver : IStatementVisitor, IExpressionVisitor<GSharpObject>
{
    Evaluator Interpreter;
    private Stack<Scope> Scopes { get; set; }
    public Resolver(Evaluator evaluator)
    {
        Scopes = new();
        Interpreter = evaluator;
    }
    private void BeginScope() => Scopes.Push(new Scope());
    private void EndScope() => Scopes.Pop();
    private void DeclareVariable(string variableName)
    {
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
        for (int i = Scopes.Count - 1; i >= 0; i--)
        {
            if (Scopes.ElementAt(i).Variables.ContainsKey(variableName))
            {
                Interpreter.ResolveReference(expr, Scopes.Count - 1 - i);
                return;
            }
        }
    }
    private void BindFunctionbody(GSharpExpression expr, string functionName)
    {
        for (int i = Scopes.Count - 1; i >= 0; i--)
        {
            if (Scopes.ElementAt(i).Functions.ContainsKey(functionName))
            {
                Interpreter.ResolveReference(expr, Scopes.Count - 1 - i);
                return;
            }
        }
    }
    public GSharpObject VisitBinaryOperation(BinaryOperation binary)
    {
        binary.LeftArgument.Accept(this);
        binary.RightArgument.Accept(this);
        return null;
    }
    public GSharpObject VisitConstant(Constant constant)
    {
        if (Scopes.Count == 0)
            ErrorHandler.AddError(new DefaultError("Local variable is not defined", "semantic"));
        if (Scopes.Peek().Variables[constant.Name] == false)
            ErrorHandler.AddError(new DefaultError("Can't read local variable in its own initializer", "semantic"));
        BindValue(constant, constant.Name);
        return null;
    }
    public void VisitConstantDeclaration(ConstantsDeclaration declaration)
    {
        if (declaration.ConstantNames.Count == 1)
        {
            DeclareVariable(declaration.ConstantNames[0]);
            declaration.Value.Accept(this);
            DefineVariable(declaration.ConstantNames[0]);
            return;
        }
        if (declaration.Value is not LiteralSequence)
            throw new Exception("Match declaration");
        LiteralSequence sequence = (LiteralSequence)declaration.Value;
        int index = 0;
        foreach (GSharpExpression expression in sequence.Expressions)
        {
            if (index == declaration.ConstantNames.Count - 1)
            {
                DeclareVariable(declaration.ConstantNames[index]);
                sequence.GetTail(index).Accept(this);
                DefineVariable(declaration.ConstantNames[index]);
                break;
            }
            DeclareVariable(declaration.ConstantNames[index]);
            expression.Accept(this);
            DefineVariable(declaration.ConstantNames[index]);
            index++;
        }
        if (index < declaration.ConstantNames.Count - 2)
        {
            for (int i = index; index < declaration.ConstantNames.Count - 1; i++)
            {
                DeclareVariable(declaration.ConstantNames[i]);
                DefineVariable(declaration.ConstantNames[i]);
            }
        }
        DeclareVariable(declaration.ConstantNames[^1]);
        sequence.GetTail(declaration.ConstantNames.Count - 1).Accept(this);
        DefineVariable(declaration.ConstantNames[^1]);


    }
    public void VisitExpressionStatement(ExpressionStatement expression) => expression.Expression.Accept(this);
    public GSharpObject VisitFunctionCall(FunctionCall functionCall)
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
    public GSharpObject VisitIfThenElse(IfThenElse ifThen)
    {
        ifThen.Condition.Accept(this);
        ifThen.IfExpression.Accept(this);
        ifThen.ElseExpression.Accept(this);
        return null;
    }
    public GSharpObject VisitLetIn(LetIn letIn)
    {
        BeginScope();
        foreach (ConstantsDeclaration declaration in letIn.DeclaredConstants)
            declaration.Accept(this);
        letIn.Body.Accept(this);
        EndScope();
        return null;
    }
    public GSharpObject VisitLiteralNumber(LiteralNumber literal) => null;
    public GSharpObject VisitUnaryOperation(UnaryOperation unary)
    {
        unary.Argument.Accept(this);
        return null;
    }
    public GSharpObject VisitLiteralSequence(LiteralSequence sequence)
    {
        foreach (GSharpExpression expression in sequence.Expressions)
            expression.Accept(this);
        return null;
    }
    public void VisitDrawStatment(DrawStatement drawStatement) => drawStatement.Expression.Accept(this);
    public void VisitColorStatent(ColorStatement color) { return; }
    public void VisitRestoreStatement(Restore restore) { return; }
    public GSharpObject VisitLiteralString(LiteralString @string) => null;
}
