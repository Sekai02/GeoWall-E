using GeoWallECompiler.Objects;

namespace GeoWallECompiler;
/// <summary>
/// Representa las secuencias en G#
/// </summary>
public abstract class GSharpSequence<T> : GSharpObject where T: GSharpObject
{
    public abstract GSharpSequence<T> GetTail(int tailBeggining);
    public override double ToValueOfTruth() => Count != 0 ? 1 : 0;
    /// <summary>
    /// IEnumerable de objetos de G# que conforman la secuencia
    /// </summary>
    public IEnumerable<T> Sequence { get; protected set; }
    /// <summary>
    /// Cantidad de elementos que contiene la secuencia
    /// </summary>
    public int? Count { get; protected set; }
}
public class ArraySequence<T> : GSharpSequence<T>, IRandomable<ArraySequence<T>>, IUserParameter<ArraySequence<T>> where T : GSharpObject, IRandomable<T>, IUserParameter<T>
{    
    /// <summary>
    /// Construye una secuencia de objetos de G#. Por ejemplo {p1,p2,p3,p4}
    /// </summary>
    /// <param name="objects">Lista de los objetos que se van a guardar en la secuencia</param>
    public ArraySequence(List<T> objects)
    {
        Sequence = objects;
        Count = objects.Count;
    }
    public static ArraySequence<T> GetRandomInstance(int limit = 500)
    {
        Random random = new();
        List<T> values = new();
        int count = random.Next(20);
        for (int i = 0; i < count; i++)
            values.Add(T.GetRandomInstance(limit));
        return new ArraySequence<T>(values);
    }

    public static new ArraySequence<T> GetInstanceFromParameters(Queue<double> parameters)
    {
        List<T> values = new();
        while(parameters.Count > 0)
            values.Add(T.GetInstanceFromParameters(parameters));
        return new ArraySequence<T>(values);
    }
    public override GSharpSequence<T> GetTail(int a) 
    {
        List<T> objects = new(ChoppSequence(a));
        return new ArraySequence<T>(objects);
    }   
    /// <summary>
    /// Funcion para contar infinitos numeros comenzando en un entero
    /// </summary>
    /// <param name="a">Numero inicial</param>
    /// <returns>IEnumerable que contiene los numeros en el rango</returns>
    
    private IEnumerable<T> ChoppSequence(int a)
    {
        int count = 0;
        foreach(T obj in Sequence)
        {
            if(count >= a)
                yield return obj;
            count++;
        }
    }
}
public class FiniteIntegerSequence : GSharpSequence<GSNumber>
{
    private readonly int startNumber;
    private readonly int endNumber;
    public FiniteIntegerSequence(GSNumber start, GSNumber end)
    {
        if (!double.IsInteger(start) || !double.IsInteger(end))
            throw new DefaultError("Sequences declaration with '...' can only take integers as limits", "Semantic");
        Sequence = FiniteRange(start, end);
        Count = Math.Abs(end - start);
    }
    private static IEnumerable<GSNumber> FiniteRange(int a, int b)
    {
        if (a < b)
            for (int i = a; i <= b; i++)
                yield return (GSNumber)i;
        else
            for (int i = b; i >= a; i--)
                yield return (GSNumber)i;
    }
    public override GSharpSequence<GSNumber> GetTail(int tailBeggining) 
    {
        if (tailBeggining > endNumber)
            return null;
        GSNumber start = (GSNumber)(startNumber + tailBeggining);
        GSNumber end = (GSNumber)endNumber;
        return new FiniteIntegerSequence(start, end);
    }
}
public class InfiniteIntegerSequence : GSharpSequence<GSNumber>
{
    private readonly int startNumber;
    public InfiniteIntegerSequence(GSNumber start)
    {
        if (!double.IsInteger(start))
            throw new DefaultError("Sequences declaration with '...' can only take integers as limits", "Semantic");
        startNumber = start;
        Sequence = InfiniteRange(start);
        Count = null;
    }
    private static IEnumerable<GSNumber> InfiniteRange(int a)
    {
        int i = a;
        while (true)
        {
            yield return (GSNumber)i;
            i++;
        }
    }
    public override GSharpSequence<GSNumber> GetTail(int tailBeggining)
    {
        GSNumber start = (GSNumber)(startNumber + tailBeggining);
        return new InfiniteIntegerSequence(start);
    }
}