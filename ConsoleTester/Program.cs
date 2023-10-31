using GeoWallECompiler;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleTester;

internal class Program
{
    static void Main(string[] args)
    {
        string path = "..\\..\\..\\..\\test.txt";
        GSharp gSharp = new();
        gSharp.RunFile(path);
        foreach (var error in gSharp.Errors)
            Console.WriteLine(error.Message);
    }
}
