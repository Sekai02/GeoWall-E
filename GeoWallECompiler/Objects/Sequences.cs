using GeoWallECompiler.Objects;
using System.Collections;

namespace GeoWallECompiler;
public interface ISequenciable
{
    public int? GetCount();
    public IEnumerable GetSequence();
    public ISequenciable AttachSequence(ISequenciable? sequence);
    public ISequenciable GetTail(int tailStart);
}
public class GSequence : GSObject, ISequenciable
{
    public GSequence(ISequenciable sequenciable)
    {
        NonGenericSequence = sequenciable.GetSequence();
        Count = sequenciable.GetCount();
    }
    public GSequence(IEnumerable nonGenericSequence, int? count)
    {
        NonGenericSequence = nonGenericSequence;
        Count = count;
    }
    /// <summary>
    /// IEnumerable de objetos de G# que conforman la secuencia
    /// </summary>
    public IEnumerable NonGenericSequence { get; protected set; }
    /// <summary>
    /// Cantidad de elementos que contiene la secuencia
    /// </summary>
    public int? Count { get; protected set; }
    public int? GetCount() => Count;
    public IEnumerable GetSequence() => NonGenericSequence;
    public ISequenciable AttachSequence(ISequenciable? sequenceToAppend)
    {
        if (sequenceToAppend is null)
            return this;
        IEnumerable rightValues;
        rightValues = sequenceToAppend.GetSequence();
        IEnumerable newSequence = JoinSequences(NonGenericSequence, rightValues);
        int? newCount = Count + sequenceToAppend.GetCount();
        return new GSequence(newSequence, newCount);
    }
    public ISequenciable GetTail(int tailStart)
    {
        IEnumerable newSequence = Skip(NonGenericSequence, tailStart);
        int? newCount = Count is null ? Count : Count - tailStart;
        return new GSequence(newSequence, newCount);
    }
    public static GSharpSequence<GSNumber> GetRandomNumbers() => new(RandomNumbers(), null);
    private static IEnumerable Skip(IEnumerable objects, int count)
    {
        int i = 0;
        foreach (var item in objects)
        {
            if (i >= count)
                yield return item;
            i++;
        }
    }
    private static IEnumerable JoinSequences(IEnumerable left, IEnumerable right)
    {
        foreach (var item in left)
            yield return item;
        foreach (var item in right)
            yield return item;
    }
    private static IEnumerable<GSNumber> RandomNumbers()
    {
        Random random = new();
        while (true)
            yield return (GSNumber)random.NextDouble();
    }
}
/// <summary>
/// Representa las secuencias en G#
/// </summary>
public class GSharpSequence<T> : GSequence where T : GSObject?
{
    public GSharpSequence(IEnumerable<T?> sequence, int? count) : base(sequence, count)
    {
        GenericSequence = sequence;
    }    
    public virtual new GSharpSequence<T> GetTail(int tailBeggining)
    {
        IEnumerable<T?> newSequence = GenericSequence.Skip(tailBeggining);
        int? newCount = Count is null? Count : Count - tailBeggining;
        return new GSharpSequence<T>(newSequence, newCount);
    }
    public IEnumerable<T?> GenericSequence { get; protected set; }
}
public class ArraySequence<T> : GSharpSequence<T>, IRandomable<ArraySequence<T>>, IUserParameter<ArraySequence<T>> where T : GSObject?, IRandomable<T>, IUserParameter<T>
{
    /// <summary>
    /// Construye una secuencia de objetos de G#. Por ejemplo {p1,p2,p3,p4}
    /// </summary>
    /// <param name="objects">Lista de los objetos que se van a guardar en la secuencia</param>
    public ArraySequence(List<T?> objects) : base(objects, objects.Count)
    {
        NonGenericSequence = objects;
        Count = objects.Count;
    }
    public static new ArraySequence<T> GetRandomInstance(int limit = 500)
    {
        Random random = new();
        List<T?> values = new();
        int count = random.Next(20);
        for (int i = 0; i < count; i++)
            values.Add(T.GetRandomInstance(limit));
        return new ArraySequence<T>(values);
    }

    public static new ArraySequence<T> GetInstanceFromParameters(Queue<double> parameters)
    {
        List<T?> values = new();
        while(parameters.Count > 0)
            values.Add(T.GetInstanceFromParameters(parameters));
        return new ArraySequence<T>(values);
    }
    public override GSharpSequence<T> GetTail(int a) 
    {
        List<T?> objects = new(ChoppSequence(a));
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
        foreach(T obj in NonGenericSequence)
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
    public FiniteIntegerSequence(GSNumber start, GSNumber end) : base(FiniteRange(start, end), Math.Abs((end - start)!)) { }
    private static IEnumerable<GSNumber> FiniteRange(GSNumber start, GSNumber end)
    {
        if (!double.IsInteger(start) || !double.IsInteger(end))
            throw new DefaultError("Sequences declaration with '...' can only take integers as limits", "Semantic");
        if (start < end)
            for (int i = start; i <= end; i++)
                yield return (GSNumber)i;
        else
            for (int i = end; i >= start; i--)
                yield return (GSNumber)i;
    }
    public override GSharpSequence<GSNumber> GetTail(int tailBeggining) 
    {
        if (tailBeggining > endNumber)
            return new GSharpSequence<GSNumber>(Array.Empty<GSNumber>(), 0);
        GSNumber start = (GSNumber)(startNumber + tailBeggining);
        GSNumber end = (GSNumber)endNumber;
        return new FiniteIntegerSequence(start, end);
    }
}
public class InfiniteIntegerSequence : GSharpSequence<GSNumber>
{
    private readonly int startNumber;
    public InfiniteIntegerSequence(GSNumber start) : base(InfiniteRange(start), null)
    {        
        startNumber = start;
    }
    private static IEnumerable<GSNumber> InfiniteRange(GSNumber start)
    {
        if (!double.IsInteger(start))
            throw new DefaultError("Sequences declaration with '...' can only take integers as limits", "Semantic");
        int i = start;
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