namespace GeoWallECompiler;
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
