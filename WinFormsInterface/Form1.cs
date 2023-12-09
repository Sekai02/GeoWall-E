using GeoWallECompiler;

namespace WinFormsInterface;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    public class Drawer : IDrawer
    {
        Graphics Graphics;
        public Drawer(Graphics graphics) => Graphics = graphics;
        public void DrawArc() => throw new NotImplementedException();
        public void DrawCircle() => throw new NotImplementedException();
        public void DrawLine() => throw new NotImplementedException();
        public void DrawPoint(GSharpPoint point)
        {
            Graphics.DrawEllipse(new Pen(Color.Black, 10), (float)point.Coordinates.Value.X.Value, (float)point.Coordinates.Value.Y.Value, (float)point.Coordinates.Value.X.Value + 100, (float)point.Coordinates.Value.Y.Value + 100);
        }
        public void DrawRay() => throw new NotImplementedException();
        public void DrawSegment() => throw new NotImplementedException();
        public void DrawSequence() => throw new NotImplementedException();
        public void DrawString(GSharpString gString) => throw new NotImplementedException();
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        Graphics g = CreateGraphics();
        Drawer drawer = new(g);
        drawer.DrawPoint(new GSharpPoint(new GSharpNumber(3), new GSharpNumber(2)));

    }
}
