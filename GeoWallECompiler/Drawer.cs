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
    public void DrawPoint(GSPoint point, GSString name = null);
    public void DrawLine(Line line, GSString name = null);
    public void DrawSegment(Segment segment, GSString name = null);
    public void DrawCircle(Circle circle, GSString name = null);
    public void DrawArc(Arc arc, GSString name = null);
    public void DrawRay(Ray ray, GSString name = null);
    public void DrawString(GSString gString);
    public void DrawSequence<T>(GSharpSequence<T> sequence) where T: GSharpObject, IDrawable;
}
/// <summary>
/// Interfaz que implementa un metodo para dibujar una figura en el plano 
/// </summary>
public interface IDrawable
{
    public void Draw(IDrawer drawer, GSString? label);
}
