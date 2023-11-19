namespace GeoWallECompiler;

/// <summary>
/// Clase abstracta de la que heredan todos los objetos de G#
/// </summary>
public abstract class GSharpObject
{
    /// <summary>
    /// Metodo que devuelve el valor de verdad de un objeto en forma de 0 o 1
    /// </summary>
    /// <returns>0 si el objeto evalua como falso y 1 en caso contrario</returns>
    public abstract double ToValueOfTruth();
}
/// <summary>
/// Clase que representa los valores numericos en G#
/// </summary>
public class GSharpNumber : GSharpObject
{
    /// <summary>
    /// Crea una instancia que representa un valor numerico en G#
    /// </summary>
    /// <param name="val"></param>
    public GSharpNumber(double val) => Value = val;
    public override double ToValueOfTruth() => Value == 0 ? 0 : 1;
    /// <summary>
    /// Valor numerico del numero
    /// </summary>
    public double Value { get; private set; }

    #region Operators Overloads
    public static GSharpNumber operator +(GSharpNumber a) => new(a.Value);
    public static GSharpNumber operator -(GSharpNumber a) => new(-a.Value);
    public static GSharpNumber operator +(GSharpNumber a, GSharpNumber b) => new(a.Value + b.Value);
    public static GSharpNumber operator -(GSharpNumber a, GSharpNumber b) => new(a.Value - b.Value);
    public static GSharpNumber operator *(GSharpNumber a, GSharpNumber b) => new(a.Value * b.Value);
    public static GSharpNumber operator /(GSharpNumber a, GSharpNumber b) => new(a.Value / b.Value);
    public static GSharpNumber operator %(GSharpNumber a, GSharpNumber b) => new(a.Value % b.Value);
    public static GSharpNumber operator <(GSharpNumber a, GSharpNumber b) => a.Value < b.Value? new(1): new(0);
    public static GSharpNumber operator >(GSharpNumber a, GSharpNumber b) => a.Value > b.Value ? new(1) : new(0);
    public static GSharpNumber operator <=(GSharpNumber a, GSharpNumber b) => a.Value <= b.Value ? new(1) : new(0);
    public static GSharpNumber operator >=(GSharpNumber a, GSharpNumber b) => a.Value >= b.Value ? new(1) : new(0);
    public static GSharpNumber operator ==(GSharpNumber a, GSharpNumber b) => a.Value == b.Value ? new(1) : new(0);
    public static GSharpNumber operator !=(GSharpNumber a, GSharpNumber b) => a.Value != b.Value ? new(1) : new(0);
    #endregion
}
/// <summary>
/// Clase que representa las cadenas de caracteres en G#
/// </summary>
public class GSharpString : GSharpObject
{
    /// <summary>
    /// Crea una instancia de una cadena de texto en G#
    /// </summary>
    /// <param name="val"></param>
    public GSharpString(string val) => Value = val;
    public override double ToValueOfTruth() => Value == "" ? 0 : 1;
    /// <summary>
    /// Valor de la cadena de texto
    /// </summary>
    public string Value { get; private set; }
}

/// <summary>
/// Representa un punto en el espacio bidimensional
/// </summary>
public class GSharpPoint : GSharpObject, IDrawable
{    
    /// <summary>
    /// Crea una instancia de un punto en el espacio bidimensional definido mediante dos coordenadas rectangulares
    /// </summary>
    /// <param name="x">Coordenada x del punto</param>
    /// <param name="y">Coordenada y del punto</param>
    public GSharpPoint(GSharpNumber? x, GSharpNumber? y)
    {        
        if (x is not null && y is not null)
            Coordinates = (x, y);
        else
            Coordinates = null;
    }
    public override double ToValueOfTruth() => Coordinates is null? 0 : 1;
    public void Draw(IDrawer drawer) => drawer.DrawPoint(this);
    /// <summary>
    /// Coordendas rectangulares del punto
    /// </summary>
    public (GSharpNumber X, GSharpNumber Y)? Coordinates { get; private set; }
}
/// <summary>
/// Representa una linea recta en el espacio bidimensional
/// </summary>
public class Line : GSharpObject, IDrawable
{
    /// <summary>
    /// Crea una instancia de una linea en el espacio bidimensional mediante dos puntos
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    public Line(GSharpPoint? point1, GSharpPoint? point2)
    {
        Point1 = point1;
        Point2 = point2;
    }
    public GSharpPoint? Point1 { get; private set; }
    public GSharpPoint? Point2 { get; private set; }
    public void Draw(IDrawer drawer) => drawer.DrawLine(this);
    public override double ToValueOfTruth() => Point1 is null || Point2 is null ? 0 : 1;
}
/// <summary>
/// Representa un segmento en el espacio bidimensional
/// </summary>
public class Segment : GSharpObject, IDrawable
{
    /// <summary>
    /// Crea una instancia de un segmento en el espacio bidimensional a partir de dos puntos (sus extremos)
    /// </summary>
    /// <param name="point1">Extremo 1</param>
    /// <param name="point2">Extremo 2</param>
    public Segment(GSharpPoint? point1, GSharpPoint? point2)
    {
        Point1 = point1;
        Point2 = point2;
    }
    /// <summary>
    /// Punto extremo 1
    /// </summary>
    public GSharpPoint? Point1 { get; private set; }
    /// <summary>
    /// Punto extremo 2
    /// </summary>
    public GSharpPoint? Point2 { get; private set; }
    public void Draw(IDrawer drawer) => drawer.DrawSegment(this);
    public override double ToValueOfTruth() => Point1 is null || Point2 is null ? 0 : 1;
}
/// <summary>
/// Representa una linea que se extiende infnitamente por solo un extremo
/// </summary>
public class Ray : GSharpObject, IDrawable
{
    /// <summary>
    /// Instancia una linea que se extiende infinitamente por un extremo a partir de un punto inicial y un segundo punto
    /// </summary>
    /// <param name="point1">Punto inicial</param>
    /// <param name="point2">Punto por donde pasa el rayo</param>
    public Ray(GSharpPoint? point1, GSharpPoint? point2)
    {
        Point1 = point1;
        Point2 = point2;
    }
    /// <summary>
    /// Punto inicial
    /// </summary>
    public GSharpPoint? Point1 { get; private set; }
    /// <summary>
    /// Punto por donde pasa el rayo
    /// </summary>
    public GSharpPoint? Point2 { get; private set; }
    public void Draw(IDrawer drawer) => drawer.DrawRay(this);
    public override double ToValueOfTruth() => Point1 is null || Point2 is null ? 0 : 1;
}
/// <summary>
/// Representa una circunferencia en el espacio bidimensional
/// </summary>
public class Circle : GSharpObject, IDrawable
{
    /// <summary>
    /// Instancia una cicunferencia en el plano definida mediante un radio y un centro
    /// </summary>
    /// <param name="center">Punto centro de la circunferencia</param>
    /// <param name="radius">Radio de la circunferencia</param>
    public Circle(GSharpPoint? center, GSharpNumber? radius)
    {
        Center = center;
        Radius = radius;
    }
    /// <summary>
    /// Punto centro de la circunferencia
    /// </summary>
    public GSharpPoint? Center { get; private set; }
    /// <summary>
    /// Radio de la circunferencia
    /// </summary>
    public GSharpNumber? Radius { get; private set; }
    public void Draw(IDrawer drawer) => drawer.DrawCircle(this);
    public override double ToValueOfTruth() => Center is null || Radius is null ? 0 : 1;
}
/// <summary>
/// Representa un arco de circunferencia en el plano
/// </summary>
public class Arc : GSharpObject, IDrawable
{
    /// <summary>
    /// Instancia un arco definido por un centro, que se extiende desde una semirecta que pasa
    /// por un punto hasta una semirecta que pasa por otro punto y un radio
    /// </summary>
    /// <param name="center">Centro del arco de la circunferencia</param>
    /// <param name="startPoint">Punto por el que pasa la recta de inicio del arco</param>
    /// <param name="endPoint">Punto por el que pasa la recta donde termina el arco</param>
    /// <param name="radius">Radio del arco</param>
    public Arc(GSharpPoint? center, GSharpPoint? startPoint, GSharpPoint? endPoint, GSharpNumber? radius)
    {
        Center = center;
        StartPoint = startPoint;
        EndPoint = endPoint;
        Radius = radius;
    }
    /// <summary>
    /// Centro del arco de la circunferencia
    /// </summary>
    public GSharpPoint? Center { get; }
    /// <summary>
    /// Punto por el que pasa la recta de inicio del arco
    /// </summary>
    public GSharpPoint? StartPoint { get; }
    /// <summary>
    /// Punto por el que pasa la recta donde termina el arco
    /// </summary>
    public GSharpPoint? EndPoint { get; }
    /// <summary>
    /// Radio del arco
    /// </summary>
    public GSharpNumber? Radius { get; private set; }
    public void Draw(IDrawer drawer) => drawer.DrawArc(this);
    public override double ToValueOfTruth() => Center is null || StartPoint is null|| EndPoint is null || Radius is null ? 0 : 1;
}

