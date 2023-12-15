
using GeoWallECompiler.StandardLibrary;
using GeoWallECompiler.Visitors;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Text;

namespace GeoWallECompiler;

/// <summary>
/// Clase que controla la logica del compilador (scanning,parsing,evaluating)
/// </summary>
public static class GSharp
{
    /// <summary>
    /// Tokeniza el código provisto en source
    /// </summary>
    /// <param name="source">Código fuente a tokenizar</param>
    public static List<Token> Scan(string source)
    {
        Scanner scanner = new Scanner(source);
        return scanner.ScanTokens();
        //foreach (Token token in tokens)
        //    Console.WriteLine(token);
    }
    /// <summary>
    /// Ejecuta el codigo desde un archivo del sistema
    /// </summary>
    /// <param name="path"></param>
    public static void RunFile(string path, IDrawer drawer, IWalleUI userInterface)
    {
        Scanner.Line = 0;
        string full = Path.GetFullPath(path);
        byte[] bytes = File.ReadAllBytes(Path.GetFullPath(path));
        Run(Encoding.Default.GetString(bytes), drawer, userInterface);

        if (Error.HadError) Environment.Exit(65);
    }

    public static Container RunLibraryFile(string path, IDrawer drawer, IWalleUI userInterface)
    {
        byte[] bytes = File.ReadAllBytes(Path.GetFullPath(path));
        return RunLibrary(Encoding.Default.GetString(bytes), drawer, userInterface);
    }

    public static Container RunLibrary(string source, IDrawer drawer, IWalleUI userInterface)
    {
        List<Token> tokens = Scan(source);
        if (ErrorHandler.HadError)
        {
            //userInterface.PrintErrors(ErrorHandler.GetErrors());
            return null!;
        }
        Parser parser = new(tokens, drawer, userInterface);
        List<Statement> statements = parser.Parse();
        if (ErrorHandler.HadError)
        {
            //userInterface.PrintErrors(ErrorHandler.GetErrors());
            return null!;
        }
        Evaluator evaluator = new(drawer, userInterface);
        Resolver resolver = new(evaluator);
        TypeChecker typeChecker = new(evaluator);
        resolver.VisitStatements(statements);
        if (ErrorHandler.HadError)
        {
            //userInterface.PrintErrors(ErrorHandler.GetErrors());
            return null!;
        }
        typeChecker.VisitStatements(statements);
        if (ErrorHandler.HadError)
        {
            //userInterface.PrintErrors(ErrorHandler.GetErrors());
            return null!;
        }
        try
        {
            evaluator.VisitStatements(statements);
            return new Container(evaluator.EvaluationContext, typeChecker.TypeEnvironment, resolver.Scopes.Peek());
        }
        catch (GSharpException ex)
        {
            //userInterface.PrintError(ex);
            return null!;
        }
    }

    /// <summary>
    /// Ejecuta una sola linea de codigo
    /// </summary>
    /// <param name="source"></param>
    private static void Run(string source, IDrawer drawer, IWalleUI userInterface)
    {
        CanvasHeight = drawer.CanvasHeight;
        CanvasWidth = drawer.CanvasWidth;
        drawer.Reset();
        List<Token> tokens = Scan(source);
        if (ErrorHandler.HadError)
        {
            userInterface.PrintErrors(ErrorHandler.GetErrors());
            return;
        }
        Parser parser = new(tokens, drawer, userInterface);
        List<Statement> statements = parser.Parse();
        if (ErrorHandler.HadError)
        {
            userInterface.PrintErrors(ErrorHandler.GetErrors());
            return;
        }
        Evaluator evaluator = new(drawer, userInterface);
        Resolver resolver = new(evaluator);
        TypeChecker typeChecker = new(evaluator);
        resolver.VisitStatements(statements);
        if (ErrorHandler.HadError)
        {
            userInterface.PrintErrors(ErrorHandler.GetErrors());
            return;
        }
        typeChecker.VisitStatements(statements);
        if (ErrorHandler.HadError)
        {
            userInterface.PrintErrors(ErrorHandler.GetErrors());
            return;
        }
        try
        {
            evaluator.VisitStatements(statements);
        }
        catch (GSharpException ex)
        {
            userInterface.PrintError(ex);
            return;
        }
        if (ErrorHandler.HadError)
        {
            userInterface.PrintErrors(ErrorHandler.GetErrors());
            return;
        }
    }
    public static int CanvasWidth { get; set; }
    public static int CanvasHeight { get; set; }
    public static void InitializeGSharpStandard<V>(Context<V, ICallable> context)
    {
        context.SetFunction("point", new PointGetter());
        context.SetFunction("line", new LineGetter());
        context.SetFunction("segment", new SegmentGetter());
        context.SetFunction("ray", new RayGetter());
        context.SetFunction("arc", new ArcGetter());
        context.SetFunction("circle", new CircleGetter());
        context.SetFunction("measure", new MeasureFunction());
        context.SetFunction("count", new CountFunction());
        context.SetFunction("randoms", new RandomsFunction());
        context.SetFunction("points", new PointsFunction());
        context.SetFunction("samples", new SamplesFunction());
        context.SetFunction("intersect", new IntersectFunction());
    }
    public static void InitializeGSharpStandard(Context<bool, bool> scope)
    {
        scope.SetFunction("point", true);
        scope.SetFunction("line", true);
        scope.SetFunction("segment", true);
        scope.SetFunction("ray", true);
        scope.SetFunction("arc", true);
        scope.SetFunction("circle", true);
        scope.SetFunction("measure", true);
        scope.SetFunction("count", true);
        scope.SetFunction("randoms", true);
        scope.SetFunction("points", true);
        scope.SetFunction("samples", true);
        scope.SetFunction("intersect", true);
    }
}