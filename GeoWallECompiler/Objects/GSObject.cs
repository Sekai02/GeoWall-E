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
    public abstract double ToValueOfTruth();
    public bool IsSequence = false;
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
    public override double ToValueOfTruth() => Value == 0 ? 0 : 1;
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
    public static GSNumber operator +(GSNumber a) => a;
    public static GSNumber operator -(GSNumber a) => -a;
    public static GSNumber operator +(GSNumber a, GSNumber b) => a + b;
    public static GSNumber operator -(GSNumber a, GSNumber b) => a - b;
    public static GSNumber operator *(GSNumber a, GSNumber b) => a * b;
    public static GSNumber operator /(GSNumber a, GSNumber b) => a / b;
    public static GSNumber operator %(GSNumber a, GSNumber b) => a % b;
    public static bool operator <(GSNumber a, GSNumber b) => a < b;
    public static bool operator >(GSNumber a, GSNumber b) => a > b;
    public static bool operator <=(GSNumber a, GSNumber b) => a <= b;
    public static bool operator >=(GSNumber a, GSNumber b) => a >= b;
    public static bool operator ==(GSNumber a, GSNumber b) => a == b;
    public static bool operator !=(GSNumber a, GSNumber b) => a != b;
    public static implicit operator double(GSNumber n) => n.Value;
    public static implicit operator int(GSNumber n) => (int)n.Value;
    public static implicit operator float(GSNumber n) => (float)n.Value;
    public static explicit operator GSNumber(double d) => new(d);
    public static explicit operator GSNumber(int n) => new(n);
    #endregion
}
/// <summary>
/// Clase que representa las cadenas de caracteres en G#
/// </summary>
public class GSString : GSObject
{
    /// <summary>
    /// Crea una instancia de una cadena de texto en G#
    /// </summary>
    /// <param name="val"></param>
    public GSString(string val) => Value = val;
    public override double ToValueOfTruth() => Value == "" ? 0 : 1;
    /// <summary>
    /// Valor de la cadena de texto
    /// </summary>
    public string Value { get; private set; }
    public static implicit operator string(GSString s) => s.Value;
    public static explicit operator GSString(string s) => new(s);
}
/// <summary>
/// Clase que representa un objeto medida
/// </summary>
public class Measure : GSObject, IRandomable<Measure>, IUserParameter<Measure>
{
    public Measure(GSPoint p1, GSPoint p2)
    {
        Point1 = p1;
        Point2 = p2;
        double x1 = p1.Coordinates.Value.X;
        double x2 = p2.Coordinates.Value.X;
        double y1 = p1.Coordinates.Value.Y;
        double y2 = p2.Coordinates.Value.Y;
        Value = (GSNumber)Math.Sqrt(Math.Pow(x2-x1, 2) + Math.Pow(y2-y1,2));
    }
    public GSPoint Point1 { get; }
    public GSPoint Point2 { get; }
    public GSNumber Value { get; }
    public static Measure GetRandomInstance(int limit) 
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
    public override double ToValueOfTruth() => 1;
}
