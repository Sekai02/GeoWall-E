namespace GeoWallECompiler;
public class Drawer
{
    public Drawer(DrawFunc drawFunc) => Draw = drawFunc;
    public DrawFunc Draw { get; private set; }
    public delegate void DrawFunc(IDrawable figure);
}
public interface IDrawable
{
    public void Draw();
}
