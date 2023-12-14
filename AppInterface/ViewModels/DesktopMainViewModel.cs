using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeoWallECompiler;
using MetalPerformanceShaders;
using Microsoft.Maui.Graphics;
using System.Collections;
namespace AppInterface.ViewModels;

public partial class DesktopMainViewModel : ObservableObject
{
    [ObservableProperty]
    string terminalText;
    [ObservableProperty]
    string codeText;
    [ObservableProperty]
    string inputText;
    private string programPath;
    public DesktopMainViewModel()
    {
    }

    [RelayCommand]
    private async void OpenFolder()
    {
        var file = await PickFile();
        programPath = file;
    }   

    async Task<string> PickFile()
    {
        var result = await FilePicker.PickAsync(default);
        if (result is not null)
        {
            string path = result.FullPath;
            return path;
        }
        else
        {
            return null;
        }
    }
}
public class Figure : Microsoft.Maui.Graphics.IDrawable
{
    public Figure(GeoWallECompiler.IDrawable drawable) 
    {
        Drawable = drawable;
    }
    public GeoWallECompiler.IDrawable Drawable { get; }
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
    }
}
public class CanvasDrawer : IDrawer
{
    private readonly int height;
    private readonly int weight;
    private Stack<Color> usedColors;
    private Color currentColor;

    public CanvasDrawer(ICanvas drawer, int height, int weight)
    {
        Drawer = drawer;
        Drawer.ResetStroke();
        currentColor = Colors.Black;
        this.height = height;
        this.weight = weight;
    }
    public int CanvasHeight => height;

    public int CanvasWidth => weight;

    public ICanvas Drawer { get; }

    public void DrawArc(Arc arc, GString name = null)
    {

    }
    public void DrawCircle(Circle circle, GString name = null) => throw new NotImplementedException();
    public void DrawEnumerable(IEnumerable values) => throw new NotImplementedException();
    public void DrawLine(Line line, GString name = null) => throw new NotImplementedException();
    public void DrawPoint(GSPoint point, GString name = null) => throw new NotImplementedException();
    public void DrawRay(Ray ray, GString name = null) => throw new NotImplementedException();
    public void DrawSegment(Segment segment, GString name = null) => throw new NotImplementedException();
    public void DrawSequence<T>(GSharpSequence<T> sequence) where T : GSObject, GeoWallECompiler.IDrawable => throw new NotImplementedException();
    public void DrawString(GString gString) => throw new NotImplementedException();
    public void Reset() 
    {
        usedColors.Clear();
        Drawer.StrokeColor = currentColor = Colors.Black;
    }
    public void ResetColor() 
    {
        if (usedColors.TryPop(out Color oldColor))
            Drawer.StrokeColor = currentColor = oldColor;
    }
    public void SetColor(string newColor)
    {
        usedColors.Push(currentColor);
        Drawer.StrokeColor = currentColor = Color.Parse(newColor);
    }
}
