using System;
using System.Linq.Expressions;
using System.Text;

namespace GeoWallECompiler;

/// <summary>
/// Clase que controla la logica del compilador (scanning,parsing,evaluating)
/// </summary>
public static class GSharp
{
    public static void Scan(string source)
    {
        Scanner scanner=new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();
        foreach (Token token in tokens)
            Console.WriteLine(token);
    }
    /// <summary>
    /// Ejecuta el codigo desde un archivo del sistema
    /// </summary>
    /// <param name="path"></param>
    public static void RunFile(string path)
    {
        Scanner.Line = 0;

        byte[] bytes = File.ReadAllBytes(Path.GetFullPath(path));
        Run(Encoding.Default.GetString(bytes));

        if (Error.HadError) Environment.Exit(65);
    }

    /// <summary>
    /// Ejecuta una sola linea de codigo
    /// </summary>
    /// <param name="source"></param>
    private static void Run(string source)
    {
        Scan(source);
    }
}