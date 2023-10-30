using GeoWallECompiler;

namespace ConsoleTester;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("! EXECUTION ERROR: Number of args should be less than 2");
            Environment.Exit(64);
        }
        else if (args.Length == 1)
        {
            GSharp.RunFile(args[0]);
        }
        else
        {
            GSharp.RunPrompt();
        }
    }
}
