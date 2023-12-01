using System.Drawing;

namespace GeoWallECompiler;
/// <summary>
/// Interfaz que implementa metodos para dibujar figuras en el plano
/// </summary>
public interface IDrawer
{
    public void Reset();
    public void SetColor(string newColor);
    public void ResetColor();
    public void DrawPoint(GSharpPoint point, GSharpString name = null);
    public void DrawLine(Line line, GSharpString name = null);
    public void DrawSegment(Segment segment, GSharpString name = null);
    public void DrawCircle(Circle circle, GSharpString name = null);
    public void DrawArc(Arc arc, GSharpString name = null);
    public void DrawRay(Ray ray, GSharpString name = null);
    public void DrawString(GSharpString gString);
    public void DrawSequence<T>(GSharpSequence<T> sequence) where T: GSharpObject, IDrawable;
}
/// <summary>
/// Interfaz que implementa un metodo para dibujar una figura en el plano 
/// </summary>
public interface IDrawable
{
    public void Draw(IDrawer drawer, GSharpString? label);
}
