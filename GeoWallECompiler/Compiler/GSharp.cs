using System;
using System.Linq.Expressions;
using System.Text;

namespace GeoWallECompiler;

/// <summary>
/// Clase que controla la logica del compilador (scanning,parsing,evaluating)
/// </summary>
public class GSharp
{
    /// <summary>
    /// Ejecuta el codigo desde un archivo del sistema
    /// </summary>
    /// <param name="path"></param>
    public static void RunFile(string path)
    {
        Scanner.Line = 0;
        byte[] bytes = File.ReadAllBytes(Path.GetFullPath(path));
        RunLines(Encoding.Default.GetString(bytes));

        if (Error.HadError) Environment.Exit(65);
    }

    /// <summary>
    /// Ejecuta el codigo desde la consola
    /// </summary>
    public static void RunPrompt()
    {
        Scanner.Line = 0;
        while (true)
        {
            Console.Write("[{0}] > ", Scanner.Line + 1);
            string Line = Console.ReadLine()!;
            if (Line == null) break;
            Run(Line);
            Error.HadError = false;
        }
    }

    /// <summary>
    /// Ejecuta varias lineas (es llamada por RunFile para ejecutar las distintas
    /// lineas de un archivo)
    /// </summary>
    /// <param name="source"></param>
    private static void RunLines(string source)
    {
        List<string> lines = new List<string>();
        string cur = "";

        foreach (char c in source)
        {
            if (c == '\n' || c == '\r')
            {
                if (cur.Length > 0) lines.Add(cur);
                cur = "";
            }
            else cur = cur + c;
        }
        if (cur.Length > 0) lines.Add(cur);

        foreach (string line in lines)
        {
            Run(line);
        }
    }

    /// <summary>
    /// Ejecuta una sola linea de codigo
    /// </summary>
    /// <param name="source"></param>
    private static void Run(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();

        if (Error.HadError) return;

        Console.WriteLine("[{0}]", Scanner.Line);
        foreach (Token token in tokens)
        {
            Console.WriteLine(token);
        }
    }
}