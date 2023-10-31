using System;
using System.Linq.Expressions;
using System.Text;

namespace GeoWallECompiler;

/// <summary>
/// Clase que controla la logica del compilador (scanning,parsing,evaluating)
/// </summary>
public class GSharp
{
    public List<GSharpException> Errors { get; set; }
    public Scanner Scanner { get; private set; }
    public GSharp()
    {
        Errors = new();
    }
    public void Scan(string source)
    {
        Scanner = new(source, Errors);
        List<Token> tokens = Scanner.ScanTokens();
        foreach (Token token in tokens)
            Console.WriteLine(token);
    }
    /// <summary>
    /// Ejecuta el codigo desde un archivo del sistema
    /// </summary>
    /// <param name="path"></param>
    public void RunFile(string path)
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
    private void Run(string source)
    {
        Scan(source);
    }
}