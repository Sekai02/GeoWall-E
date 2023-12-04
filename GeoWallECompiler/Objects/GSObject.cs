using GeoWallECompiler.Objects;

namespace GeoWallECompiler;

/// <summary>
/// Clase abstracta de la que heredan todos los objetos de G#
/// </summary>
public abstract class GSObject : IRandomable<GSObject>, IUserParameter<GSObject>
{
    public static GSObject GetRandomInstance(int limit = 500) => GSNumber.GetRandomInstance(limit);
    public static GSObject GetInstanceFromParameters(Queue<double> parameters) => GSNumber.GetInstanceFromParameters(parameters);

    /// <summary>
    /// Metodo que devuelve el valor de verdad de un objeto en forma de 0 o 1
    /// </summary>
    /// <returns>0 si el objeto evalua como falso y 1 en caso contrario</returns>
    public static GSNumber ToValueOfTruth(GSObject? gObject)
    {
        if (gObject is null)
            return (GSNumber)0;
        if (gObject is GSequence sequence)
            return sequence.Count != 0? (GSNumber)1 : (GSNumber)0;
        if (gObject is GSNumber number)
            return number != 0 ? (GSNumber)1 : (GSNumber)0;
        return (GSNumber)1;
    }
    public override string ToString() => "G# object";
}
/// <summary>
/// Clase que representa los valores numericos en G#
/// </summary>
public class GSNumber : GSObject, IRandomable<GSNumber>, IUserParameter<GSNumber>
{
    /// <summary>
    /// Crea una instancia que representa un valor numerico en G#
    /// </summary>
    /// <param name="val"></param>
    public GSNumber(double val) => Value = val;
    public override string ToString() => Value.ToString();
    public static new GSNumber GetRandomInstance(int limit = 500)
    {
        Random random = new();
        return (GSNumber)random.Next(limit);
    }
    public static new GSNumber GetInstanceFromParameters(Queue<double> parameters)
    {
        if (!parameters.TryDequeue(out double number))
        {
            Random random = new();
            return (GSNumber)random.Next(500);
        }
        return (GSNumber)number;
    }
    /// <summary>
    /// Valor numerico del numero
    /// </summary>
    public double Value { get; private set; }
    #region Operators Overloads
    
    public static GSNumber? operator +(GSNumber? a) => a;
    public static GSNumber? operator -(GSNumber? a) => -a;
    public static GSNumber? operator +(GSNumber? a, GSNumber? b) => (GSNumber?)(a?.Value + b?.Value);
    public static GSNumber? operator -(GSNumber? a, GSNumber? b) => (GSNumber?)(a?.Value - b?.Value);
    public static GSNumber? operator *(GSNumber? a, GSNumber? b) => (GSNumber?)(a?.Value * b?.Value);
    public static GSNumber? operator /(GSNumber? a, GSNumber? b) => (GSNumber?)(a?.Value / b?.Value);
    public static GSNumber? operator %(GSNumber? a, GSNumber? b) => (GSNumber?)(a?.Value % b?.Value);
    public static bool operator <(GSNumber? a, GSNumber? b) => a?.Value < b?.Value;
    public static bool operator >(GSNumber? a, GSNumber? b) => a?.Value > b?.Value;
    public static bool operator <=(GSNumber? a, GSNumber? b) => a?.Value <= b?.Value;
    public static bool operator >=(GSNumber? a, GSNumber? b) => a?.Value >= b?.Value;
    public static bool operator ==(GSNumber? a, GSNumber? b) => a?.Value == b?.Value;
    public static bool operator !=(GSNumber? a, GSNumber? b) => a?.Value != b?.Value;
    public static implicit operator double(GSNumber n) => n.Value;
    public static implicit operator int(GSNumber n) => (int)n.Value;
    public static implicit operator float(GSNumber n) => (float)n.Value;
    public static implicit operator double?(GSNumber? n) => n?.Value;
    public static implicit operator int?(GSNumber? n) => (int?)n?.Value;
    public static implicit operator float?(GSNumber? n) => (float?)n?.Value;
    public static explicit operator GSNumber(double d) => new(d);
    public static explicit operator GSNumber(int n) => new(n);
    #endregion
}
/// <summary>
/// Clase que representa las cadenas de caracteres en G#
/// </summary>
public class GString : GSObject
{
    /// <summary>
    /// Crea una instancia de una cadena de texto en G#
    /// </summary>
    /// <param name="val"></param>
    public GString(string val) => Value = val;
    public override string ToString() => Value;
    /// <summary>
    /// Valor de la cadena de texto
    /// </summary>
    public string Value { get; private set; }
    public static implicit operator string(GString s) => s.Value;
    public static explicit operator GString(string s) => new(s);
}
/// <summary>
/// Clase que representa un objeto medida
/// </summary>
public class Measure : GSObject, IRandomable<Measure>, IUserParameter<Measure>
{
    public Measure(GSPoint p1, GSPoint p2)
    {
        double x1 = p1.Coordinates.X;
        double x2 = p2.Coordinates.X;
        double y1 = p1.Coordinates.Y;
        double y2 = p2.Coordinates.Y;
        Lenght = (GSNumber)Math.Sqrt(Math.Pow(x2-x1, 2) + Math.Pow(y2-y1,2));
    }
    public Measure(GSNumber value)
    {
        Lenght = value;
        if(Lenght < 0)
        {
            Lenght = new(-Lenght.Value);
        }
    }

    public GSNumber Lenght { get; }
    public static new Measure GetRandomInstance(int limit = 500) 
    {
        GSPoint point1 = GSPoint.GetRandomInstance();
        GSPoint point2 = GSPoint.GetRandomInstance();
        return new Measure(point1, point2);
    }
    public static new Measure GetInstanceFromParameters(Queue<double> parameters)
    {
        GSPoint point1 = GSPoint.GetInstanceFromParameters(parameters);
        GSPoint point2 = GSPoint.GetInstanceFromParameters(parameters);
        return new Measure(point1, point2);
    }
    public override string ToString() => "measure " + Lenght;
    public static Measure? operator +(Measure? a, Measure? b)
    {
        var value = a?.Lenght + b?.Lenght;
        return value is null ? null : new(value);
    }

    public static Measure? operator -(Measure? a, Measure? b)
    {
        var value = a?.Lenght - b?.Lenght;
        return value is null ? null : new(value);
    }
    public static Measure? operator *(Measure? a, int? n)
    {
        if (a is null || n is null)
            return null;
        var value = (GSNumber?)(a.Lenght * n);
        return value is null ? null : new(value);
    }

    public static Measure? operator *(int? n, Measure? a)
    {
        if (a is null || n is null)
            return null;
        var value = (GSNumber?)(a.Lenght * n);
        return value is null ? null : new(value);
    }
    public static Measure? operator /(Measure? a, Measure? b)
    {
        if (a is null || b is null)
            return null;
        GSNumber? division = a.Lenght / b.Lenght;
        if (division is null)
            return null;
        return new((GSNumber)(int)division!);
    }
    public static bool operator <(Measure? a, Measure? b) => a?.Lenght < b?.Lenght;
    public static bool operator >(Measure? a, Measure? b) => a?.Lenght > b?.Lenght;
    public static bool operator <=(Measure? a, Measure? b) => a?.Lenght <= b?.Lenght;
    public static bool operator >=(Measure? a, Measure? b) => a?.Lenght >= b?.Lenght;
    public static bool operator ==(Measure? a, Measure? b) => a?.Lenght == b?.Lenght;
    public static bool operator !=(Measure? a, Measure? b) => a?.Lenght != b?.Lenght;
}
