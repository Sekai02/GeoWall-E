using GeoWallECompiler;

namespace GWalleWFUI;
public class PictureDrawer : IDrawer
{
    private readonly Graphics drawer;
    private Stack<Color> usedColors;
    public PictureDrawer(Graphics graphics, Pen pen)
    {
        drawer = graphics;
        DrawerPen = pen;
        usedColors = new();
    }
    public Pen DrawerPen { get; }
    private void WriteMessage(GSharpString message, double x, double y)
    {
        string s = message?.Value ?? "";
        if (s != "")
            drawer.DrawString(s, new Font("Arial", 10), DrawerPen.Brush, (float)x + 3, (float)y + 3);
    }
    private double GetLineAngleDeg(double x1, double y1, double x2, double y2)
    {
        double y = (y2 - y1);
        double x = (x2 - x1);
        double angle = Math.Atan2(y, x);
        return angle;
    }
    public void DrawArc(Arc arc, GSharpString name = null)
    {
        double xc = arc.Center.Coordinates.Value.X.Value;
        double yc = arc.Center.Coordinates.Value.Y.Value;
        double x1 = arc.StartPoint.Coordinates.Value.X.Value;
        double y1 = arc.StartPoint.Coordinates.Value.Y.Value;
        double x2 = arc.EndPoint.Coordinates.Value.X.Value;
        double y2 = arc.EndPoint.Coordinates.Value.Y.Value;
        double radius = arc.Radius.Value;

        double startAngle = GetLineAngleDeg(xc, yc, x1, y1);
        double endAngle = GetLineAngleDeg(xc, yc, x2, y2);

        if (startAngle > endAngle)
            endAngle += 360;
        double sweepAngle = endAngle - startAngle;
        sweepAngle -= 360;
        double topLeftX = xc - radius;
        double topLeftY = yc - radius;
        drawer.DrawArc(DrawerPen, (float)topLeftX, (float)topLeftY, 2 * (float)radius, 2 * (float)radius, (float)startAngle, (float)sweepAngle);

        WriteMessage(name, xc, yc);
    }
    public void DrawCircle(Circle circle, GSharpString name = null)
    {
        float x = (float)circle.Center.Coordinates.Value.X.Value;
        float y = (float)circle.Center.Coordinates.Value.Y.Value;
        float radius = (float)circle.Radius.Value;
        drawer.DrawEllipse(DrawerPen, x - radius, y - radius, 2 * radius, 2 * radius);

        WriteMessage(name, x, y);
    }
    public void DrawLine(Line line, GSharpString name = null)
    {
        double x1 = line.Point1.Coordinates.Value.X.Value;
        double y1 = line.Point1.Coordinates.Value.Y.Value;
        double x2 = line.Point2.Coordinates.Value.X.Value;
        double y2 = line.Point2.Coordinates.Value.Y.Value;

        if (x1 == x2)
        {
            float finalX = (float)x1;
            float verticalY1 = 0;
            float verticalY2 = 10000;
            drawer.DrawLine(DrawerPen, finalX, verticalY1, finalX, verticalY2);
            return;
        }
        double m = (y2 - y1) / (x2 - x1);
        double n = y1 - m * x1;
        double finalX1 = 0;
        double finalY1 = n;
        double finalX2 = 10000;
        double finalY2 = 10000 * m + n;
        drawer.DrawLine(DrawerPen, (float)finalX1, (float)finalY1, (float)finalX2, (float)finalY2);

        double xMessage = (finalX1 + finalX2) / 2;
        double yMessage = (finalY1 + finalY2) / 2;
        WriteMessage(name, xMessage, yMessage);
    }
    public void DrawPoint(GSharpPoint point, GSharpString name = null)
    {
        float x = (float)point.Coordinates.Value.X.Value;
        float y = (float)point.Coordinates.Value.Y.Value;
        drawer.DrawEllipse(DrawerPen, x, y, 2, 2);

        WriteMessage(name, x, y);
    }
    public void DrawRay(Ray ray, GSharpString name = null)
    {
        double x1 = ray.Point1.Coordinates.Value.X.Value;
        double y1 = ray.Point1.Coordinates.Value.Y.Value;
        double x2 = ray.Point2.Coordinates.Value.X.Value;
        double y2 = ray.Point2.Coordinates.Value.Y.Value;

        if (x1 == x2)
        {
            double verticalY = x1 < x2 ? 10000 : 0;
            drawer.DrawLine(DrawerPen, (float)x1, (float)x2, (float)x2, (float)verticalY);
            return;
        }
        double m = (y2 - y1) / (x2 - x1);
        double n = y1 - m * x1;
        double finalX;
        double finalY;
        if (x1 < x2)
        {
            finalX = 10000;
            finalY = 10000 * m + n;
        }
        else
        {
            finalX = 0;
            finalY = n;
        }
        drawer.DrawLine(DrawerPen, (float)x1, (float)y1, (float)finalX, (float)finalY);

        WriteMessage(name, x1, y1);
    }
    public void DrawSegment(Segment segment, GSharpString name = null)
    {
        float x1 = (float)segment.Point1.Coordinates.Value.X.Value;
        float y1 = (float)segment.Point1.Coordinates.Value.Y.Value;
        float x2 = (float)segment.Point2.Coordinates.Value.X.Value;
        float y2 = (float)segment.Point2.Coordinates.Value.Y.Value;
        drawer.DrawLine(DrawerPen, x1, y1, x2, y2);

        double xMessage = (x1 + y1) / 2;
        double yMessage = (x2 + y2) / 2;
        WriteMessage(name, xMessage, yMessage);
    }
    public void DrawSequence<T>(GSharpSequence<T> sequence) where T : GSharpObject, IDrawable
    {
        foreach (var obj in sequence.Sequence)
            obj.Draw(this);
    }
    public void DrawString(GSharpString gString)
    {
        Font font = new("Arial", 16);
        Brush brush = new SolidBrush(Color.Black);
        drawer.DrawString(gString.Value, font, brush, 0, 0);
        font.Dispose();
        brush.Dispose();
    }
    public void SetColor(Color newColor)
    {
        usedColors.Push(DrawerPen.Color);
        DrawerPen.Color = newColor;        
    }
    public void ResetColor()
    {
        if (usedColors.TryPop(out Color oldColor))
             DrawerPen.Color = oldColor;
    }
}