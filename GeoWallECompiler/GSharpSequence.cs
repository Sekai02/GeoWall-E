namespace GeoWallECompiler;
/// <summary>
/// Representa las secuencias en G#
/// </summary>
public class GSharpSequence : GSharpObject
{    
    /// <summary>
    /// Construye una secuencia de objetos de G#. Por ejemplo {p1,p2,p3,p4}
    /// </summary>
    /// <param name="objects">Lista de los objetos que se van a guardar en la secuencia</param>
    public GSharpSequence(List<GSharpObject> objects)
    {
        Sequence = objects;
        Count = objects.Count;
    }
    /// <summary>
    /// Construye una secuencia de un rango infinito de enteros que empiezan por un numero indicado. Por ejemplo {1...}
    /// </summary>
    /// <param name="start">Numero inicial</param>
    /// <exception cref="DefaultError"></exception>
    public GSharpSequence(GSharpNumber start)
    {
        if (!double.IsInteger(start.Value))
            throw new DefaultError("Sequences declaration with '...' can only take integers as limits", "Semantic");
        Sequence = InfiniteRange((int)start.Value);
        Count = null;
    }
    /// <summary>
    /// Construye una secuencia de un rango de enteros desde un número hasta otro. Por ejemplo {1...100}
    /// </summary>
    /// <param name="start">Numero inicial</param>
    /// <param name="end">Numero final</param>
    /// <exception cref="DefaultError"></exception>
    public GSharpSequence(GSharpNumber start, GSharpNumber end)
    {
        if (!double.IsInteger(start.Value) || !double.IsInteger(end.Value))
            throw new DefaultError("Sequences declaration with '...' can only take integers as limits", "Semantic");
        Sequence = FiniteRange((int)start.Value, (int)end.Value);
        Count = (int)Math.Abs(end.Value - end.Value);
    }
    /// <summary>
    /// Funcion para contar numeros desde en un rango de enteros
    /// </summary>
    /// <param name="a">Numero inicial</param>
    /// <param name="b">Numero final</param>
    /// <returns>IEnumerable que contiene los numeros en el rango</returns>
    private static IEnumerable<GSharpNumber> FiniteRange(int a, int b)
    {
        if (a < b)
            for(int i = a; i <=b; i++)
                yield return new GSharpNumber(i);
        else 
            for(int i = b; i>= a; i--)
                yield return new GSharpNumber(i);
    }
    /// <summary>
    /// Funcion para contar infinitos numeros comenzando en un entero
    /// </summary>
    /// <param name="a">Numero inicial</param>
    /// <returns>IEnumerable que contiene los numeros en el rango</returns>
    private static IEnumerable<GSharpNumber> InfiniteRange(int a)
    {
        int i = a;
        while (true)
        {
            yield return new GSharpNumber(i);
            i++;
        }
    }
    public override double ToValueOfTruth() => Count != 0? 1 : 0;
    /// <summary>
    /// IEnumerable de objetos de G# que conforman la secuencia
    /// </summary>
    public IEnumerable<GSharpObject> Sequence;
    /// <summary>
    /// Cantidad de elementos que contiene la secuencia
    /// </summary>
    public int? Count { get; private set; }
}
