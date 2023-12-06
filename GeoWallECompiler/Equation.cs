namespace GeoWallECompiler;
/// <summary>
/// Representa una ecuacion lineal o una de segundo grado que representa un circulo
/// </summary>
public class Equation
{
    private ParameterConstrains? thetaConstrain;
    private readonly GSPoint? point;
    public Equation(GSPoint point)
    {
        IsPoint = true;
        this.point = point;
    }
    public Equation(double ax, double bx, double c, bool isCircular = false)
    {
        AlphaX = ax;
        AlphaY = bx;
        C = c;
        IsCircular = isCircular;

    }
    public Equation(double ax, double bx, double c, ParameterConstrains? constrainX = null, ParameterConstrains? constrainY = null)
    {
        AlphaX = ax;
        AlphaY = bx;
        C = c;
        ConstrainsX = constrainX;
        ConstrainsY = constrainY;
        IsCircular = false;        
    }
    public Equation(double ax, double bx, double c, ParameterConstrains? constrainTheta = null)
    {
        AlphaX = ax;
        AlphaY = bx;
        C = c;
        IsCircular = true;
        thetaConstrain = constrainTheta is null? new("theta", -Math.PI, Math.PI) : constrainTheta;
    }
    public GSPoint? Point => IsPoint ? point : null; 
    public bool IsPoint { get; set; }
    public double AlphaX2 => IsCircular ? 1 : 0;
    public double AlphaY2 => IsCircular ? 1 : 0;
    public double AlphaX { get; }
    public double AlphaY { get; }
    public double C { get; }
    public bool IsCircular { get; }
    public ParameterConstrains? ConstrainsX { get; }
    public ParameterConstrains? ConstrainsY { get; }
    public ParameterConstrains ThetaConstrain => IsCircular ? thetaConstrain! : new("theta", 0, 2 * Math.PI);
    public static Equation GetLineEquation(GSPoint p1, GSPoint p2, ParameterConstrains? constrainsX = null, ParameterConstrains? constrainsY = null)
    {
        // Definir los puntos por los que pasa la recta
        double x1 = p1.Coordinates.X;
        double y1 = p1.Coordinates.Y;

        double x2 = p2.Coordinates.X;
        double y2 = p2.Coordinates.Y;

        // Calcular los coeficientes A, B y C de la ecuación
        double A = y2 - y1;
        double B = x1 - x2;
        double C = (x2 * y1) - (x1 * y2);

        return new(A, B, C, constrainsX, constrainsY);
    }
    public static Equation GetCircleEquation(GSPoint center, Measure radius, ParameterConstrains? angleConstrain = null)
    {
        double h = center.Coordinates.X;
        double k = center.Coordinates.Y;
        double r = (double)radius.Lenght;
        double alphaX = -2 * h;
        double alphaY = -2 * k;
        double c = h * h + k * k - r * r;
        return new(alphaX, alphaY, c, angleConstrain);
    }
}
public class SimpleCuadraticEquation
{
    public SimpleCuadraticEquation(double a, double b, double c)
    {
        Ax2 = a;
        Bx = b;
        C = c;
    }

    public double Ax2 { get; }
    public double Bx { get; }
    public double C { get; }
    public List<double> Solve()
    {
        List<double> result = new();
        double discriminant = Bx * Bx - 4 * Ax2 * C;
        if(discriminant > 0)
        {
            double x1 = (-Bx + Math.Sqrt(discriminant)) / (2 * Ax2);
            double x2 = (-Bx - Math.Sqrt(discriminant)) / (2 * Ax2);
            result.Add(x1);
            result.Add(x2);
            return result;
        }
        if(discriminant == 0)
        {
            double x = -Bx / (2 * Ax2);
            result.Add(x);
            return result;
        }
        return result;
    }
}
public static class EquationSolver
{
    public static IEnumerable<GSPoint> SolveCircularSystem(Equation circular1, Equation circular2)
    {
        List<GSPoint> results;
        if (!circular1.IsCircular && !circular2.IsCircular)
            results = SolveLinearSystem(circular1, circular2);
        else if (!circular1.IsCircular)
            results = SolveCircularLinearSystem(circular2, circular1);
        else if (!circular2.IsCircular)
            results = SolveCircularLinearSystem(circular1, circular2);
        else
        {
            double a = circular1.AlphaX - circular2.AlphaX;
            double b = circular1.AlphaY - circular2.AlphaY;
            double c = circular1.C - circular2.C;

            Equation equation = new(a, b, c, false);
            results = SolveCircularLinearSystem(circular1, equation);
        }
        return FilterInRangeSolutions(results, circular1, circular2);
    }
    private static List<GSPoint> SolveLinearSystem(Equation linear1, Equation linear2)
    {
        // Definir los coeficientes de las ecuaciones
        double a1 = linear1.AlphaX;
        double b1 = linear1.AlphaY;
        double c1 = -linear1.C;

        double a2 = linear2.AlphaX;
        double b2 = linear2.AlphaY;
        double c2 = -linear2.C;

        // Calcular la solución del sistema de ecuaciones
        double determinant = a1 * b2 - a2 * b1;

        if (determinant == 0)
            return new();

        double x = (b2 * c1 - b1 * c2) / determinant;
        double y = (a1 * c2 - a2 * c1) / determinant;

        List<GSPoint> result = new() { new GSPoint((GSNumber)x, (GSNumber)y) };
        return result;
    }
    public static List<GSPoint> SolveCircularLinearSystem(Equation circular, Equation linear)
    {
        double a = linear.AlphaX;
        double b = linear.AlphaY;
        double c = linear.C;
        if (a == 0 && b == 0)
            return new();
        SimpleCuadraticEquation cuadratic;
        List<GSPoint> results = new();
        if (a == 0)
        {
            double y = - c / b;
            var aloneY = new Equation(0, 0, y, false);
            cuadratic = SubstituteVariable(aloneY, circular, false);
            List<double> xi = cuadratic.Solve();
            
            foreach (double x in xi)
            {
                results.Add(new ((GSNumber)x, (GSNumber)y));
            }
            return results;
        }
        var aloneX = new Equation(0, -b / a, -c / a, false);
        cuadratic = SubstituteVariable(aloneX, circular, true); ;
        List<double> yi = cuadratic.Solve();
        if(b == 0)
        {
            double x = -c / a;
            foreach (double y in yi)
                results.Add(new((GSNumber)x, (GSNumber)y));
            return results;
        }
        foreach (double y in yi)
        {
            double x = -(b * y + c)/a;
            results.Add(new((GSNumber)x, (GSNumber)y));
        }
        return results;
    }
    private static SimpleCuadraticEquation SubstituteVariable(Equation parameter, Equation equation, bool isXValue)
    {
        double a;
        double b;
        double c;

        if (isXValue)
        {
            a = 1 + Math.Pow(parameter.AlphaY, 2);
            b = 2 * parameter.AlphaY * parameter.C + equation.AlphaX * parameter.AlphaY + equation.AlphaY;
            c = Math.Pow(parameter.C, 2) + equation.AlphaX * parameter.C + equation.C;
        }
        else
        {
            a = 1 + Math.Pow(parameter.AlphaX, 2);
            b = 2 * parameter.AlphaX * parameter.C + equation.AlphaY * parameter.AlphaX + equation.AlphaX;
            c = Math.Pow(parameter.C, 2) + equation.AlphaY * parameter.C + equation.C;
        }
        return new SimpleCuadraticEquation(a, b, c);
    }
    private static double EvaluateEcuation(double x, double y, Equation equation)
    {
        double a = equation.AlphaX;
        double b = equation.AlphaY;
        double c = equation.C;
        if (equation.IsCircular)
        {
            return (x * x) + (y * y) + (a * x) + (b * x) + c; 
        }
        return (a * x) + (b * y) + c;
    }
    private static IEnumerable<GSPoint> FilterInRangeSolutions(IEnumerable<GSPoint> posibleSolutions, Equation circular1, Equation circular2)
    {
        List<GSPoint> result = new();
        foreach (var point in posibleSolutions)
        {
            double x = point.Coordinates.X;
            double y = point.Coordinates.Y;
            ParameterConstrains?[] constrainsX = { circular1.ConstrainsX, circular2.ConstrainsX };
            if (!ParameterConstrains.AcceptAllConstrains(x, constrainsX))
                continue;
            ParameterConstrains?[] constrainsY = { circular1.ConstrainsY, circular2.ConstrainsY };
            if (!ParameterConstrains.AcceptAllConstrains(y, constrainsY))
                continue;
            if (circular1.IsCircular)
            {
                double xc = circular1.AlphaX / (-2);
                double yc = circular1.AlphaY / (-2);
                double theta = Math.Atan2(y - yc, x - xc);
                ParameterConstrains thetaConstrain = circular1.ThetaConstrain;
                if (!thetaConstrain.IsInRange(theta) && !thetaConstrain.IsInRange(theta + 2 * Math.PI))
                    continue;
            }
            if (circular2.IsCircular)
            {
                double xc = circular2.AlphaX / (-2);
                double yc = circular2.AlphaY / (-2);
                double theta = Math.Atan2(y - yc, x - xc);
                var thetaConstrain = circular2.ThetaConstrain;
                if (!thetaConstrain.IsInRange(theta) && !thetaConstrain.IsInRange(theta + 2 * Math.PI))
                    continue;
            }
            result.Add( point);
        }
        return result;
    }
}
public class ParameterConstrains
{
    public ParameterConstrains(string parameterName, double lowerBound, double upperBound)
    {
        ParameterName = parameterName;
        LowerBound = lowerBound < upperBound ? lowerBound : upperBound;
        UpperBound = upperBound > lowerBound ? upperBound : lowerBound;
    }
    public bool IsInRange(double parameter) => parameter >= LowerBound && parameter <= UpperBound;
    public static bool AcceptAllConstrains(double parameter,ParameterConstrains?[] constrains)
    {
        foreach (var constrain in constrains)
        {
            if(constrain is not null)
                if (!constrain.IsInRange(parameter))
                    return false;
        }
        return true;
    }
    public string ParameterName { get; }
    public double LowerBound { get; }
    public double UpperBound { get; }
}
