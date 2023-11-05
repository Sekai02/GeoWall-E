namespace GeoWallECompiler;

/// <summary>
/// Representa un valor explicitamente en el codigo. Por ejemplo 3, "hello world", {1,2,3}
/// </summary>
public class Literal : GSharpExpression
{
    /// <summary>
    /// Construye un objeto que representa un valor literal
    /// </summary>
    /// <param name="value">Objeto valor de la expresion</param>
    public Literal(GSharpObject value)
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
            GSharpSequence => GSharpTypes.GSequence,
            _ => throw new DefaultError("Invalid literal"),
        };
    }
    /// <summary>
    /// Valor del literal
    /// </summary>
    public GSharpObject Value { get; private set; }
    /// <summary>
    /// Tipo del literal
    /// </summary>
    public GSharpTypes Type;
    public override GSharpObject? GetValue() => Value;
    public override GSharpTypes CheckType() => Type;
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
    public Constant(string name, GSharpExpression? value)
    {
        Name = name;
        ValueExpression = value;
        Type = SetType(value.GetValue());
    }
    /// <summary>
    /// Funcion que retorna el tipo de la constante en funcion del valor de este
    /// </summary>
    /// <param name="value">Valor de la constante</param>
    /// <returns>Tipo de la constante</returns>
    /// <exception cref="DefaultError"></exception>
    private static GSharpTypes SetType(GSharpObject? value)
    {
        return value switch
        {
            null => GSharpTypes.Undetermined,
            GSharpNumber => GSharpTypes.GNumber,
            GSharpString => GSharpTypes.GString,
            GSharpSequence => GSharpTypes.GSequence,
            _ => throw new DefaultError("Invalid literal"),
        };
    }
    /// <summary>
    /// Nombre de la constante
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// Expresion del valor de la constante
    /// </summary>
    public GSharpExpression? ValueExpression
    {
        get => ValueExpression;
        set
        {
            ValueExpression = value;
            Type = SetType(ValueExpression?.GetValue());
        }
    }
    /// <summary>
    /// Tipo de la constante
    /// </summary>
    public GSharpTypes Type { get; private set; }
    public override GSharpTypes CheckType() => Type;
    public override GSharpObject? GetValue() => ValueExpression?.GetValue();
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
    public FunctionCall(string name, List<GSharpExpression> arguments /*aqui va la declaracion de la funcion*/)
    {
        FunctionName = name;
        Arguments = arguments;
        //aqui hay que igualar la declaracion de la funcion con el parametro conrrespondiente
    }
    /// <summary>
    /// Chequea los tipos de los argumentos de un llamado a funcion con el objetivo de capturar posibles errores semanticos
    /// </summary>
    /// <param name="arguments">Lista de argumentos</param>
    private static void CheckArgs(List<GSharpExpression> arguments)
    {
        foreach (var argument in arguments)
            argument.CheckType();
    }
    public override GSharpTypes CheckType()
    {
        CheckArgs(Arguments);
        //Chequear la definicion de la funcion y retornar su valor
        return GSharpTypes.Undetermined;
    }
    public override GSharpObject? GetValue() => throw new NotImplementedException() /*aqui va el llamado a funcion.evaluate*/;
    /// <summary>
    /// Nombre de la funcion que se llama
    /// </summary>
    public string FunctionName { get; private set; }
    /// <summary>
    /// Lista de argumentos que toma la funcion
    /// </summary>
    public List<GSharpExpression> Arguments { get; private set; }
    //aqui va una propiedad publica que es la expresion de la definicion de la funcion
}
