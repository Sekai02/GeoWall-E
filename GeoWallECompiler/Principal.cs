﻿namespace GeoWallECompiler;

/// <summary>
/// Representa un valor explicitamente en el codigo. Por ejemplo 3, "hello world", {1,2,3}
/// </summary>
public class LiteralNumber : GSharpExpression
{
    /// <summary>
    /// Construye un objeto que representa un valor literal
    /// </summary>
    /// <param name="value">Objeto valor de la expresion</param>
    public LiteralNumber(GSharpNumber value)
    {
        Value = value;
        Type = SetType(value);
    }
    /// <summary>
    /// Funcion que asigna el tipo del literal en funcion del valor de este
    /// </summary>
    /// <param name="value">Valor del literal</param>
    /// <returns>Tipo del literal</returns>
    /// <exception cref="DefaultError"></exception>
    private static GSharpTypes SetType(GSharpObject value)
    {
        return value switch
        {
            GSharpNumber => GSharpTypes.GNumber,
            GSharpString => GSharpTypes.GString,
        };
    }

    public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitLiteral(this);

    /// <summary>
    /// Valor del literal
    /// </summary>
    public GSharpNumber Value { get; private set; }
    /// <summary>
    /// Tipo del literal
    /// </summary>
    public GSharpTypes Type;
}
/// <summary>
/// Representa a las constantes de G#
/// </summary>
public class Constant : GSharpExpression
{
    /// <summary>
    /// Construye un objeto que representa una constante de G#
    /// </summary>
    /// <param name="value">Valor que toma la constante</param>
    public Constant(string name) => Name = name;
    public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitConstant(this);
    /// <summary>
    /// Nombre de la constante
    /// </summary>
    public string Name { get; private set; }
}
/// <summary>
/// Representa a los llamados a funcion de G#
/// </summary>
public class FunctionCall : GSharpExpression
{
    /// <summary>
    /// Construye un objeto que representa un llamado a funcion
    /// </summary>
    /// <param name="name">Nombre de la funcion que se está llamando</param>
    /// <param name="arguments">Lista de argumentos que toma el llamado de funcion</param>
    public FunctionCall(string name, List<GSharpExpression> arguments)
    {
        FunctionName = name;
        Arguments = arguments;
    }
    public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitFunctionCall(this);
    /// <summary>
    /// Chequea los tipos de los argumentos de un llamado a funcion con el objetivo de capturar posibles errores semanticos
    /// </summary>
    /// <param name="arguments">Lista de argumentos</param>
    //private static void CheckArgs(List<GSharpExpression> arguments)
    //{
    //    foreach (var argument in arguments)
    //        argument.CheckType();
    //}
    //public override GSharpTypes CheckType()
    //{
    //    CheckArgs(Arguments);
    //    //Chequear la definicion de la funcion y retornar su valor
    //    return GSharpTypes.Undetermined;
    //}
    /// <summary>
    /// Nombre de la funcion que se llama
    /// </summary>
    public string FunctionName { get; private set; }
    /// <summary>
    /// Lista de argumentos que toma la funcion
    /// </summary>
    public List<GSharpExpression> Arguments { get; private set; }
}
