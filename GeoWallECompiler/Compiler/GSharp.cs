using System;
using System.Linq.Expressions;
using System.Text;

namespace GeoWallECompiler;

public class GSharp
{
    public static void RunFile(string path)
    {
        Scanner.Line = 0;
        byte[] bytes = File.ReadAllBytes(Path.GetFullPath(path));
        Run(Encoding.Default.GetString(bytes));

        if (Error.hadError) Environment.Exit(65);
    }

    public static void RunPrompt()
    {
        Scanner.Line = 0;
        while (true)
        {
            Console.Write("[{0}] > ", Scanner.Line + 1);
            string Line = Console.ReadLine()!;
            if (Line == null) break;
            Run(Line);
            Error.hadError = false;
        }
    }

    private static void Run(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();

        if (Error.hadError) return;

        foreach (Token token in tokens)
        {
            Console.WriteLine(token);
        }
    }
}