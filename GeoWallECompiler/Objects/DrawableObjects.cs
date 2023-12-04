using GeoWallECompiler.Objects;
namespace GeoWallECompiler;

/// <summary>
/// Representa un punto en el espacio bidimensional
/// </summary>
public class GSPoint : GSObject, IDrawable, IRandomable<GSPoint>, IUserParameter<GSPoint>
{
    /// <summary>
    /// Crea una instancia de un punto en el espacio bidimensional definido mediante dos coordenadas rectangulares
    /// </summary>
    /// <param name="x">Coordenada x del punto</param>
    /// <param name="y">Coordenada y del punto</param>
    public GSPoint(GSNumber x, GSNumber y)
    {
        Coordinates = (x, y);
    }
    public override string ToString() => $"G# Point";
    public void Draw(IDrawer drawer, GString? label) => drawer.DrawPoint(this, label);
    public static new GSPoint GetRandomInstance(int limit = 500)
    {
        Random random = new();
        var x = (GSNumber)random.Next(limit);
        var y = (GSNumber)random.Next(limit);
        return new GSPoint(x, y);
    }
    public static new GSPoint GetInstanceFromParameters(Queue<double> parameters)
    {
        var x = GSNumber.GetInstanceFromParameters(parameters);
        var y = GSNumber.GetInstanceFromParameters(parameters);
        return new GSPoint(x, y);
    }
    public GSPoint GetRandomPoint() => this;

    /// <summary>
    /// Coordendas rectangulares del punto
    /// </summary>
    public (GSNumber X, GSNumber Y) Coordinates { get; private set; }
}
/// <summary>
/// Representa una linea recta en el espacio bidimensional
/// </summary>
public class Line : GSObject, IDrawable, IRandomable<Line>, IUserParameter<Line>
{
    /// <summary>
    /// Crea una instancia de una linea en el espacio bidimensional mediante dos puntos
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    public Line(GSPoint point1, GSPoint point2)
    {
        Point1 = point1;
        Point2 = point2;
        double x1 = Point1.Coordinates.X;
        double x2 = Point2.Coordinates.X;
        double y1 = Point1.Coordinates.Y;
        double y2 = Point2.Coordinates.Y;
        DirectionVector = new(Math.Abs(x2-x1), Math.Abs(y2-x1));
    }
    public GSPoint Point1 { get; private set; }
    public GSPoint Point2 { get; private set; }
    public (double x, double y) DirectionVector { get; protected set; }
    public static new Line GetInstanceFromParameters(Queue<double> parameters)
    {
        var p1 = GSPoint.GetInstanceFromParameters(parameters);
        var p2 = GSPoint.GetInstanceFromParameters(parameters);
        return new Line(p1, p2);
    }
    public static new Line GetRandomInstance(int limit = 500)
    {
        var point1 = GSPoint.GetRandomInstance(limit);
        var point2 = GSPoint.GetRandomInstance(limit);
        return new Line(point1, point2);
    }
    public override string ToString() => "G# line";
    public void Draw(IDrawer drawer, GString? label) => drawer.DrawLine(this, label);
}
/// <summary>
/// Representa un segmento en el espacio bidimensional
/// </summary>
public class Segment : GSObject, IDrawable, IRandomable<Segment>, IUserParameter<Segment>
{
    /// <summary>
    /// Crea una instancia de un segmento en el espacio bidimensional a partir de dos puntos (sus extremos)
    /// </summary>
    /// <param name="point1">Extremo 1</param>
    /// <param name="point2">Extremo 2</param>
    public Segment(GSPoint point1, GSPoint point2)
    {
        Point1 = point1;
        Point2 = point2;
    }
    /// <summary>
    /// Punto extremo 1
    /// </summary>
    public GSPoint Point1 { get; private set; }
    /// <summary>
    /// Punto extremo 2
    /// </summary>
    public GSPoint Point2 { get; private set; }
    public static new Segment GetRandomInstance(int limit = 500)
    {
        var point1 = GSPoint.GetRandomInstance(limit);
        var point2 = GSPoint.GetRandomInstance(limit);
        return new Segment(point1, point2);
    }
    public static new Segment GetInstanceFromParameters(Queue<double> parameters)
    {
        var p1 = GSPoint.GetInstanceFromParameters(parameters);
        var p2 = GSPoint.GetInstanceFromParameters(parameters);
        return new Segment(p1, p2);
    }
    public override string ToString() => "G# Segment";
    public void Draw(IDrawer drawer, GString? label) => drawer.DrawSegment(this, label);
}
/// <summary>
/// Representa una linea que se extiende infnitamente por solo un extremo
/// </summary>
public class Ray : GSObject, IDrawable, IRandomable<Ray>, IUserParameter<Ray>
{
    /// <summary>
    /// Instancia una linea que se extiende infinitamente por un extremo a partir de un punto inicial y un segundo punto
    /// </summary>
    /// <param name="point1">Punto inicial</param>
    /// <param name="point2">Punto por donde pasa el rayo</param>
    public Ray(GSPoint point1, GSPoint point2)
    {
        Point1 = point1;
        Point2 = point2;
    }
    /// <summary>
    /// Punto inicial
    /// </summary>
    public GSPoint Point1 { get; private set; }
    /// <summary>
    /// Punto por donde pasa el rayo
    /// </summary>
    public GSPoint Point2 { get; private set; }
    public static new Ray GetRandomInstance(int limit = 500)
    {
        var point1 = GSPoint.GetRandomInstance(limit);
        var point2 = GSPoint.GetRandomInstance(limit);
        return new Ray(point1, point2);
    }
    public static new Ray GetInstanceFromParameters(Queue<double> parameters)
    {
        var p1 = GSPoint.GetInstanceFromParameters(parameters);
        var p2 = GSPoint.GetInstanceFromParameters(parameters);
        return new Ray(p1, p2);
    }
    public override string ToString() => "G# Ray";
    public void Draw(IDrawer drawer, GString? label) => drawer.DrawRay(this, label);
}
/// <summary>
/// Representa una circunferencia en el espacio bidimensional
/// </summary>
public class Circle : GSObject, IDrawable, IRandomable<Circle>, IUserParameter<Circle>
{
    /// <summary>
    /// Instancia una cicunferencia en el plano definida mediante un radio y un centro
    /// </summary>
    /// <param name="center">Punto centro de la circunferencia</param>
    /// <param name="radius">Radio de la circunferencia</param>
    public Circle(GSPoint center, Measure radius)
    {
        Center = center;
        Radius = radius;
    }
    /// <summary>
    /// Punto centro de la circunferencia
    /// </summary>
    public GSPoint Center { get; private set; }
    /// <summary>
    /// Radio de la circunferencia
    /// </summary>
    public Measure Radius { get; private set; }
    public static new Circle GetRandomInstance(int limit = 500)
    {
        var center = GSPoint.GetRandomInstance(limit);
        var radius = Measure.GetRandomInstance(limit);
        return new Circle(center, radius);
    }
    public static new Circle GetInstanceFromParameters(Queue<double> parameters)
    {
        var center = GSPoint.GetInstanceFromParameters(parameters);
        var radius = Measure.GetInstanceFromParameters(parameters);
        return new Circle(center, radius);
    }
    public void Draw(IDrawer drawer, GString? label) => drawer.DrawCircle(this, label);
    public override string ToString() => "G# Circle";
}
/// <summary>
/// Representa un arco de circunferencia en el plano
/// </summary>
public class Arc : GSObject, IDrawable, IRandomable<Arc>, IUserParameter<Arc>
{
    /// <summary>
    /// Instancia un arco definido por un centro, que se extiende desde una semirecta que pasa
    /// por un punto hasta una semirecta que pasa por otro punto y un radio
    /// </summary>
    /// <param name="center">Centro del arco de la circunferencia</param>
    /// <param name="startPoint">Punto por el que pasa la recta de inicio del arco</param>
    /// <param name="endPoint">Punto por el que pasa la recta donde termina el arco</param>
    /// <param name="radius">Radio del arco</param>
    public Arc(GSPoint center, GSPoint startPoint, GSPoint endPoint, Measure radius)
    {
        Center = center;
        StartPoint = startPoint;
        EndPoint = endPoint;
        Radius = radius;
    }
    /// <summary>
    /// Centro del arco de la circunferencia
    /// </summary>
    public GSPoint Center { get; }
    /// <summary>
    /// Punto por el que pasa la recta de inicio del arco
    /// </summary>
    public GSPoint StartPoint { get; }
    /// <summary>
    /// Punto por el que pasa la recta donde termina el arco
    /// </summary>
    public GSPoint EndPoint { get; }
    /// <summary>
    /// Radio del arco
    /// </summary>
    public Measure Radius { get; private set; }
    public static new Arc GetRandomInstance(int limit = 500)
    {
        var center = GSPoint.GetRandomInstance(limit);
        var startPoint = GSPoint.GetRandomInstance(limit);
        var endPoint = GSPoint.GetRandomInstance(limit);
        var radius = Measure.GetRandomInstance(limit);
        return new Arc(center, startPoint, endPoint, radius);
    }
    public static new Arc GetInstanceFromParameters(Queue<double> parameters)
    {
        var center = GSPoint.GetInstanceFromParameters(parameters);
        var startPoint = GSPoint.GetInstanceFromParameters(parameters);
        var endPoint = GSPoint.GetInstanceFromParameters(parameters);
        var radius = Measure.GetInstanceFromParameters(parameters);
        return new Arc(center, startPoint, endPoint, radius);
    }
    public void Draw(IDrawer drawer, GString? label) => drawer.DrawArc(this, label); 
    public override string ToString() => "G# Arc";
}
