namespace GeoWallECompiler;
public interface IDrawer
{
    public void DrawPoint(GSharpPoint point);
    public void DrawLine();
    public void DrawSegment();
    public void DrawCircle();
    public void DrawArc();
    public void DrawRay();
    public void DrawString(GSharpString gString);
    public void DrawSequence();
}
public interface IDrawable
{
    public void Draw(IDrawer drawer);
}
