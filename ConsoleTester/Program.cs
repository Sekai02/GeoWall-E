using GeoWallECompiler;

namespace ConsoleTester;

internal class Program
{
    static void Main(string[] args)
    {
        //string path = "..\\..\\..\\..\\test.txt";
        //GSharp.RunFile(path);

        //foreach (var error in ErrorHandler.GetErrors())
        //    Console.WriteLine(error.Message);
        Evaluator evaluator = new();
        Constant x = new("x");
        Literal one = new(new GSharpNumber(1));
        Addition sum = new(x, one);
        FunctionDeclaration functionDeclaration = new("suc", new List<string>() { "x" }, sum);
        functionDeclaration.Accept(evaluator);
        FunctionCall call = new("suc", new List<GSharpExpression>() { new Literal(new GSharpNumber(1)) });
        var result = call.Accept(evaluator);
        var number = result as GSharpNumber;
        Console.WriteLine(number.Value);
    }
}