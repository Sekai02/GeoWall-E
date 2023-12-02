using GeoWallECompiler;
using System.Reflection.Metadata.Ecma335;

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
}
public class DrawerDummie : IDrawer
{
    public DrawerDummie() { }
    public void DrawArc(Arc arc, GSharpString name = null) => Console.WriteLine("draw succeded");
    public void DrawCircle(Circle circle, GSharpString name = null) => Console.WriteLine("draw succeded");
    public void DrawLine(Line line, GSharpString name = null) => Console.WriteLine("draw succeded");
    public void DrawPoint(GSharpPoint point, GSharpString name = null) => Console.WriteLine("draw succeded");
    public void DrawRay(Ray ray, GSharpString name = null) => Console.WriteLine("draw succeded");  
    public void DrawSegment(Segment segment, GSharpString name = null) => Console.WriteLine("draw succeded");
    public void DrawSequence<T>(GSharpSequence<T> sequence) where T : GSharpObject, IDrawable => Console.WriteLine("draw succeded");
    public void DrawString(GSharpString gString) => Console.WriteLine("draw succeded");
    public void Reset() => Console.WriteLine("reset succeded");
    public void ResetColor() => Console.WriteLine("reset color succeded");
    public void SetColor(string newColor) => Console.WriteLine("set color succeded");
}