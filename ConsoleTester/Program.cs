using GeoWallECompiler;
using System.Collections;

namespace ConsoleTester;

internal class Program : IWalleUI
{
    static void Main(string[] args)
    {
        string path = "..\\..\\..\\..\\test.txt";
        GSharp.RunFile(path, new DrawerDummie());

        foreach (var error in ErrorHandler.GetErrors())
            Console.WriteLine(error.Message);
    }
    public void Print(object obj) => Console.WriteLine(obj);
}
public class DrawerDummie : IDrawer
{
    public DrawerDummie() { }

    public int CanvasHeight => 1000;
    public int CanvasWidth => 1000;
    public void DrawArc(Arc arc, GSString name = null) => Console.WriteLine("draw succeded");
    public void DrawCircle(Circle circle, GSString name = null) => Console.WriteLine("draw succeded");
    public void DrawEnumerable(IEnumerable values) => Console.WriteLine("draw succeded");
    public void DrawLine(Line line, GSString name = null) => Console.WriteLine("draw succeded");
    public void DrawPoint(GSPoint point, GSString name = null) => Console.WriteLine("draw succeded");
    public void DrawRay(Ray ray, GSString name = null) => Console.WriteLine("draw succeded");  
    public void DrawSegment(Segment segment, GSString name = null) => Console.WriteLine("draw succeded");
    public void DrawSequence<T>(GSharpSequence<T> sequence) where T : GSObject, IDrawable => Console.WriteLine("draw succeded");
    public void DrawString(GSString gString) => Console.WriteLine("draw succeded");
    public void Reset() => Console.WriteLine("reset succeded");
    public void ResetColor() => Console.WriteLine("reset color succeded");
    public void SetColor(string newColor) => Console.WriteLine("set color succeded");
}