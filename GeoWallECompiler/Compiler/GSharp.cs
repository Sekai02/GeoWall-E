
using GeoWallECompiler.Visitors;
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
    public static void RunFile(string path, IDrawer drawer)
    {
        Scanner.Line = 0;

        byte[] bytes = File.ReadAllBytes(Path.GetFullPath(path));
        Run(Encoding.Default.GetString(bytes), drawer);

        if (Error.HadError) Environment.Exit(65);
    }

    /// <summary>
    /// Ejecuta una sola linea de codigo
    /// </summary>
    /// <param name="source"></param>
    private static void Run(string source, IDrawer drawer)
    {
        drawer.Reset();
        List<Token> tokens = Scan(source);
        if (ErrorHandler.HadError)
            return;
        Parser parser = new(tokens);        
        List<Statement> statements = parser.Parse();
        if (ErrorHandler.HadError)
            return;
        Evaluator evaluator = new(drawer);
        Resolver resolver = new(evaluator);
        TypeChecker typeChecker = new(evaluator);
        resolver.VisitStatements(statements);
        if (ErrorHandler.HadError)
            return;
        typeChecker.VisitStatements(statements);
        if (ErrorHandler.HadError)
            return;
        evaluator.VisitStatements(statements);
        if (ErrorHandler.HadError)
            return;
    }
}