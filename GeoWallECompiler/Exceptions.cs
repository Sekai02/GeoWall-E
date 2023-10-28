using System.Globalization;

namespace GeoWallECompiler;

public abstract class GSharpException : Exception
{
    public override string Message => (MessageStart + MessageDefinition + ".").Replace("Gtring", "string");
    public string MessageStart { get; protected set; }
    public string MessageDefinition { get; protected set; }
}
public class InstrucctionError : GSharpException
{
    public InstrucctionError(GSharpException ex, int instrucctionNumber, int instrucctionsCount)
    {
        MessageStart = ex.MessageStart;
        string messageEnd = instrucctionsCount > 1 ? $" (on instrucction {instrucctionNumber})" : "";
        MessageDefinition = ex.MessageDefinition + messageEnd;
    }
}
public class DefaultError : GSharpException
{
    public DefaultError(string message)
    {
        MessageStart = "! ERROR : ";
        MessageDefinition = message;
    }
    public DefaultError(string message, string errorEspecification)
    {
        errorEspecification = errorEspecification.ToUpper(new CultureInfo("en-US"));
        MessageStart = $"! {errorEspecification} ERROR :";
        MessageDefinition = message;
    }
}
public class LexicalError : GSharpException
{
    public LexicalError(string invalidToken)
    {
        MessageStart = "! LEXICAL ERROR : ";
        InvalidToken = invalidToken;
        ExpectedToken = "token";
        MessageDefinition = $"`{InvalidToken}` is not a valid {ExpectedToken}";

    }
    public LexicalError(string invalidToken, string expectedToken)
    {
        MessageStart = "! LEXICAL ERROR : ";
        InvalidToken = invalidToken;
        ExpectedToken = expectedToken;
        MessageDefinition = $"`{InvalidToken}` is not a valid {ExpectedToken}";
    }
    public string InvalidToken { get; }
    public string ExpectedToken { get; }
}
public class SyntaxError : GSharpException
{
    public SyntaxError(string missingPart, string place)
    {
        MessageStart = "! SYNTAX ERROR : ";
        MissingPart = missingPart;
        MissingPlace = place;
        MessageDefinition = $"Missing {MissingPart} in {MissingPlace}";
    }
    public string MissingPart { get; }
    public string MissingPlace { get; }
}
public class SemanticError : GSharpException
{
    public SemanticError(string expression, string expected, string received)
    {
        MessageStart = "! SEMANTIC ERROR : ";
        Expression = expression;
        ExpressionReceived = received;
        ExpressionExpected = expected;
        MessageDefinition = $"{Expression} receives `{expected}`, not `{received}`";
    }
    public string Expression { get; }
    public string ExpressionReceived { get; }
    public string ExpressionExpected { get; }
}
public class OperationSemanticError : GSharpException
{
    public OperationSemanticError(string operation, string leftArg, string rightArg, string expected)
    {
        MessageStart = "! SEMANTIC ERROR : ";
        Operation = operation;
        LeftArgument = leftArg;
        RightArgument = rightArg;
        Expected = expected;
        MessageDefinition = $"Operator `{operation}` cannot be applied to operands of type  `{leftArg}` and `{rightArg}`";
    }
    public string Operation { get; }
    public string LeftArgument { get; }
    public string RightArgument { get; }
    public string Expected { get; }
}
public class OverFlowError : GSharpException
{
    public OverFlowError(string functionName)
    {
        FunctionName = functionName;
        MessageStart = "! FUNCTION ERROR : ";
        MessageDefinition = $"Function '{FunctionName}' reached call stack limit (callstack limit is {1000})";
    }
    public string FunctionName { get; }
}
