namespace GeoWallECompiler;
/// <summary>
/// Interfaz que implementa metodos para dibujar figuras en el plano
/// </summary>
public interface IDrawer
{
    public void DrawPoint(Point point);
    public void DrawLine(Line line);
    public void DrawSegment(Segment segment);
    public void DrawCircle(Circle circle);
    public void DrawArc(Arc arc);
    public void DrawRay(Ray ray);
    public void DrawString(GSharpString gString);
    public void DrawSequence();
}
/// <summary>
/// Interfaz que implementa un metodo para dibujar una figura en el plano 
/// </summary>
public interface IDrawable
{
    public void Draw(IDrawer drawer);
}
