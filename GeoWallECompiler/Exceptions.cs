﻿using System.Globalization;

namespace GeoWallECompiler;

/// <summary>
/// Clase abstrascta de la que heredan las excepciones lanzadas por el lenguaje G#
/// </summary>
public abstract class GSharpException : Exception
{
    /// <summary>
    /// Mensaje de la excepcion
    /// </summary>
    public override string Message
    {
        get
        {
            string line = LineNumber is null ? "" : $"(line {LineNumber})";
            string result = MessageStart + MessageDefinition + line  + ".";
            result = result.Replace("GNumber", "number");
            result = result.Replace("GString", "string");
            result = result.Replace("GSequence", "sequence");
            result = result.Replace("GObject", "object");
            result = result.Replace("GSPoint", "point");
            result = result.Replace("GSObject", "object");
            return result;
        }
    }
    /// <summary>
     /// Inicio del mensaje de la excepcion
     /// </summary>
    public string MessageStart { get; protected set; }
    /// <summary>
    /// Definicion del mensaje de la excepcion
    /// </summary>
    public string MessageDefinition { get; protected set; }
    /// <summary>
    /// Numero de la linea en la que ocurre el error
    /// </summary>
    public int? LineNumber { get; protected set; }
}
/// <summary>
/// Representa una excepcion capturada en una linea o instruccion
/// </summary>
public class InstrucctionError : GSharpException
{
    /// <summary>
    /// Crea una excepcion capturada en una linea o instruccion
    /// </summary>
    /// <param name="ex">Excepcion capturada</param>
    /// <param name="instrucctionNumber">Numero de linea en la que fue capturada la excepcion</param>
    /// <param name="instrucctionsCount">Cantidad de instrucciones del programa</param>
    public InstrucctionError(GSharpException ex, int instrucctionNumber, int instrucctionsCount)
    {
        MessageStart = ex.MessageStart;
        string messageEnd = instrucctionsCount > 1 ? $" (on instrucction {instrucctionNumber})" : "";
        MessageDefinition = ex.MessageDefinition + messageEnd;
    }
}
/// <summary>
/// Error Generico de G#
/// </summary>
public class DefaultError : GSharpException
{
    /// <summary>
    /// Instancia un error generico de G# sin ninguna especificacion
    /// </summary>
    /// <param name="message">Mensaje del error</param>
    public DefaultError(string message, int? lineNumber = null)
    {
        MessageStart = "! ERROR : ";
        MessageDefinition = message;
        LineNumber = lineNumber;
    }
    /// <summary>
    /// Instancia un error generico de G# con especificacion
    /// </summary>
    /// <param name="message">Mensaje del error</param>
    /// <param name="errorEspecification">Especificacion del tipo de error</param>
    /// <param name="lineNumber">Numero de la linea en la que ocurre el error</param>
    public DefaultError(string message, string errorEspecification, int? lineNumber = null)
    {
        errorEspecification = errorEspecification.ToUpper(new CultureInfo("en-US"));
        MessageStart = $"! {errorEspecification} ERROR : ";
        MessageDefinition = message;
        LineNumber = lineNumber;

    }
}
/// <summary>
/// Representa un error lexico de G#. Se producen por la presencia de tokens invalidos
/// </summary>
public class LexicalError : GSharpException
{
    /// <summary>
    /// Instancia un error lexico de G# sin especificar el token esperado
    /// </summary>
    /// <param name="invalidToken">Token incorrecto</param>
    /// <param name="lineNumber">Numero de la linea en la que ocurre el error</param>
    public LexicalError(string invalidToken, int? lineNumber = null)
    {
        MessageStart = "! LEXICAL ERROR : ";
        InvalidToken = invalidToken;
        ExpectedToken = "token";
        MessageDefinition = $"`{InvalidToken}` is not a valid {ExpectedToken}";
        LineNumber = lineNumber;

    }
    /// <summary>
    /// Instancia un error lexico de G# especificando el token esperado
    /// </summary>
    /// <param name="invalidToken">Token incorrecto</param>
    /// <param name="expectedToken">Token esperado</param>
    /// <param name="lineNumber">Numero de la linea en la que ocurre el error</param>
    public LexicalError(string invalidToken, string expectedToken, int? lineNumber = null)
    {
        MessageStart = "! LEXICAL ERROR : ";
        InvalidToken = invalidToken;
        ExpectedToken = expectedToken;
        MessageDefinition = $"`{InvalidToken}` is not a valid {ExpectedToken}";
        LineNumber = lineNumber;
    }
    /// <summary>
    /// Token invalido
    /// </summary>
    public string InvalidToken { get; }
    /// <summary>
    /// Token esperado
    /// </summary>
    public string ExpectedToken { get; }
}
/// <summary>
/// Representa un error sintactico de G#. Se produce por epresiones imcompletas
/// </summary>
public class SyntaxError : GSharpException
{
    /// <summary>
    /// Instancia un error sintactico, producido por un parte faltante en una expresion
    /// </summary>
    /// <param name="missingPart">Miembro de la expresion faltante</param>
    /// <param name="place">Nombre de la expresion incompleta</param>
    /// <param name="lineNumber">Numero de la linea en la que ocurre el error</param>
    public SyntaxError(string missingPart, string place, int? lineNumber = null)
    {
        MessageStart = "! SYNTAX ERROR : ";
        MissingPart = missingPart;
        MissingPlace = place;
        MessageDefinition = $"Missing {MissingPart} in {MissingPlace}";
        LineNumber = lineNumber;
    }
    /// <summary>
    /// Parte de la expresion faltante
    /// </summary>
    public string MissingPart { get; }
    /// <summary>
    /// Nombre de la expresion incompleta
    /// </summary>
    public string MissingPlace { get; }
}
/// <summary>
/// Representa un error semantico de G#. Se produce por el uso incorrecto de los tipos y argumentos
/// </summary>
public class SemanticError : GSharpException
{
    /// <summary>
    /// Instancia un error semantico
    /// </summary>
    /// <param name="expression">Tipo de expresion en la que ocurre el error</param>
    /// <param name="expected">Tipo esperado</param>
    /// <param name="received">Tipo recivido</param>
    /// <param name="lineNumber">Numero de la linea en la que ocurre el error</param>
    public SemanticError(string expression, string expected, string received, int? lineNumber = null)
    {
        MessageStart = "! SEMANTIC ERROR : ";
        Expression = expression;
        ExpressionReceived = received;
        ExpressionExpected = expected;
        MessageDefinition = $"{Expression} receives `{expected}`, not `{received}`";
        LineNumber = lineNumber;
    }
    /// <summary>
    /// Tipo de la expresion en la que ocurre el error
    /// </summary>
    public string Expression { get; }
    /// <summary>
    /// Tipo recivido
    /// </summary>
    public string ExpressionReceived { get; }
    /// <summary>
    /// Tipo esperado
    /// </summary>
    public string ExpressionExpected { get; }
}
/// <summary>
/// Representa un error producido cuando se excede el limite de llamados de una funcion
/// </summary>
public class OverFlowError : GSharpException
{
    /// <summary>
    /// Instancia un error producido cuando se excede el limite de llamados a una funcion
    /// </summary>
    /// <param name="functionName">Nombre de la funcion que excede el limite</param>
    /// <param name="lineNumber">Numero de la linea en la que ocurre el error</param>
    public OverFlowError(string functionName, int? lineNumber = null)  
    {
        FunctionName = functionName;
        MessageStart = "! FUNCTION ERROR : ";
        MessageDefinition = $"Function '{FunctionName}' reached call stack limit (callstack limit is {1000})";
        LineNumber = lineNumber;
    }
    /// <summary>
    /// Nombre de la funcion que excede el limite
    /// </summary>
    public string FunctionName { get; }
}
