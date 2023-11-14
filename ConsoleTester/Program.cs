using GeoWallECompiler;

namespace ConsoleTester;

internal class Program
{
    static void Main(string[] args)
    {
        string path = "..\\..\\..\\..\\test.txt";
        GSharp.RunFile(path);

        foreach (var error in ErrorHandler.GetErrors())
            Console.WriteLine(error.Message);
    }
}