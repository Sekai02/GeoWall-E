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
        currentScope.Variables.Add(variableName, false);
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
        currentScope.Functions.Add(variableName, false);
    }
    private void DefineFunction(string functionName)
    {
        if (Scopes.Count == 0)
            return;
        Scope currentScope = Scopes.Peek();
        currentScope.Functions[variableName] = true;
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
    private void BindFunctionbody(GSharpExpression expr, string variableName)
    {
        for (int i = Scopes.Count - 1; i >= 0; i--)
        {
            if (Scopes.ElementAt(i).Functions.ContainsKey(variableName))
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
            throw new DefaultError("Local variable is not defined");
        if(Scopes.Peek().Variables[constant.Name] == false)
        {
            ErrorHandler.AddError(new DefaultError("Can't read local variable in its own initializer."));
        }
        BindValue(constant, constant.Name);
        return null;
    }
    public void VisitConstantDeclaration(ConstantsDeclaration declaration)
    {
        if (declaration.ConstantNames.Count == 1)
        {
            DeclareVariable(declaration.ConstantNames[0]);
            declaration.Accept(this);
            DefineVariable(declaration.ConstantNames[0]);
        }
        //aqui se va a hacer para declaraciones match
    }
    public void VisitExpressionStatement(ExpressionStatement expression) 
    {
        expression.Expression.Accept(this);
    }
    public GSharpObject VisitFunctionCall(FunctionCall functionCall) 
    {
        foreach(GSharpExpression argument in functionCall.Arguments)
            argument.Accept(this);
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
    public GSharpObject VisitLiteral(Literal literal) => null;
    public GSharpObject VisitUnaryOperation(UnaryOperation unary) 
    {
        unary.Argument.Accept(this);
        return null;
    }
}
