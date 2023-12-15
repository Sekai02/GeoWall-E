using GeoWallECompiler;
using System.Collections;

namespace ConsoleTester;

internal class Program
{
    static void Main(string[] args)
    {
        string path = "/home/sekai02/Documents/School/1ro/2do semestre/Programacion/walleepublic/GeoWall-E/test.txt";
        GSharp.RunFile(path, new DrawerDummie(), new ConsoleUI());

        foreach (var error in ErrorHandler.GetErrors())
            Console.WriteLine(error.Message);
    }
}
public class ConsoleUI : IWalleUI
{
    public void Print(object? obj, string message = "") => Console.WriteLine(obj + message);
    public void PrintError(GSharpException obj) => Console.WriteLine(obj.Message);
}
public class DrawerDummie : IDrawer
{
    public DrawerDummie() { }
    public int CanvasHeight => 1000;
    public int CanvasWidth => 1000;
    public void DrawArc(Arc arc, GString name = null) => Console.WriteLine("draw succeded");
    public void DrawCircle(Circle circle, GString name = null) => Console.WriteLine("draw succeded");
    public void DrawEnumerable(IEnumerable values) => Console.WriteLine("draw succeded");
    public void DrawLine(Line line, GString name = null) => Console.WriteLine("draw succeded");
    public void DrawPoint(GSPoint point, GString name = null) => Console.WriteLine("draw succeded");
    public void DrawRay(Ray ray, GString name = null) => Console.WriteLine("draw succeded");
    public void DrawSegment(Segment segment, GString name = null) => Console.WriteLine("draw succeded");
    public void DrawSequence<T>(GSharpSequence<T> sequence) where T : GSObject, IDrawable => Console.WriteLine("draw succeded");
    public void DrawString(GString gString) => Console.WriteLine("draw succeded");
    public void Reset() => Console.WriteLine("reset succeded");
    public void ResetColor() => Console.WriteLine("reset color succeded");
    public void SetColor(string newColor) => Console.WriteLine("set color succeded");
}