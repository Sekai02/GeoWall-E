using System.Collections;

namespace GeoWallECompiler;
/// <summary>
/// Interfaz que implementa metodos para dibujar figuras en el plano
/// </summary>
public interface IDrawer
{
    public int CanvasHeight { get; }
    public int CanvasWidth { get; }
    public void Reset();
    public void SetColor(string newColor);
    public void ResetColor();
    public void DrawPoint(GSPoint point, GString name = null);
    public void DrawLine(Line line, GString name = null);
    public void DrawSegment(Segment segment, GString name = null);
    public void DrawCircle(Circle circle, GString name = null);
    public void DrawArc(Arc arc, GString name = null);
    public void DrawRay(Ray ray, GString name = null);
    public void DrawString(GString gString);
    public void DrawSequence<T>(GSharpSequence<T> sequence) where T: GSObject, IDrawable;
    public void DrawEnumerable(IEnumerable values);
}
/// <summary>
/// Interfaz que implementa un metodo para dibujar una figura en el plano 
/// </summary>
public interface IDrawable
{
    public void Draw(IDrawer drawer, GString? label);
}
