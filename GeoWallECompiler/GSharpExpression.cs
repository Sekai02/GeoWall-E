namespace GeoWallECompiler;

/// <summary>
/// Clase abstracta de la que heredaran todas las expresiones del lenguaje G#
/// </summary>
public abstract class GSharpExpression
{
    /// <summary>
    /// Metodo que devuelve el valor de la expresion de G#
    /// </summary>
    /// <returns> Resultado de evaluar la expresion, en forma de GSharpObject</returns>
    public abstract GSharpObject? GetValue();
    /// <summary>
    /// Metodo que chequea si el tipo de la expresion es correcto y si lo es devuelve el tipo de retorno de la misma
    /// </summary>
    /// <returns>Tipo de retorno de la expresion</returns>
    public abstract GSharpTypes CheckType();
}
/// <summary>
/// Tipos del lenguaje G#
/// </summary>
public enum GSharpTypes 
{
    //aqui se iran añadiendo los tipos que definamos
    GNumber,
    GString,
    GSequence,
    GObject,
    Undetermined
}