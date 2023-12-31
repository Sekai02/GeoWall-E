﻿using GeoWallECompiler;
using System.Collections;

namespace GWalleWFUI;
public class PictureDrawer : IDrawer
{
    private readonly Graphics drawer;
    private Stack<Color> usedColors;
    private readonly int _height;
    private readonly int _width;
    public PictureDrawer(Graphics graphics, Pen pen, int height, int width)
    {
        drawer = graphics;
        DrawerPen = pen;
        usedColors = new();
        _height = height;
        _width = width;
    }
    public Pen DrawerPen { get; }
    public int CanvasHeight => _height;
    public int CanvasWidth => _width;
    private void WriteMessage(GString? message, double x, double y)
    {
        string s = message?.Value ?? "";
        if (s != "")
            drawer.DrawString(s, new Font("Arial", 10), DrawerPen.Brush, (float)x + 3, (float)y + 3);
    }
    private static double GetLineAngleDeg(double x1, double y1, double x2, double y2)
    {
        double y = (y2 - y1);
        double x = (x2 - x1);
        double angle = Math.Atan2(y, x);
        return angle * (180 / Math.PI);
    }
    public void DrawArc(Arc arc, GString? name = null)
    {
        double xc = arc.Center.Coordinates.X;
        double yc = arc.Center.Coordinates.Y;
        double x1 = arc.StartPoint.Coordinates.X;
        double y1 = arc.StartPoint.Coordinates.Y;
        double x2 = arc.EndPoint.Coordinates.X;
        double y2 = arc.EndPoint.Coordinates.Y;
        double radius = arc.Radius.Lenght;

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
    public void DrawCircle(Circle circle, GString? name = null)
    {
        float x = circle.Center.Coordinates.X;
        float y = circle.Center.Coordinates.Y;
        float radius = circle.Radius.Lenght;
        drawer.DrawEllipse(DrawerPen, x - radius, y - radius, 2 * radius, 2 * radius);

        WriteMessage(name, x, y);
    }
    public void DrawLine(Line line, GString? name = null)
    {
        double x1 = line.Point1.Coordinates.X;
        double y1 = line.Point1.Coordinates.Y;
        double x2 = line.Point2.Coordinates.X;
        double y2 = line.Point2.Coordinates.Y;

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
    public void DrawPoint(GSPoint point, GString? name = null)
    {
        float x = point.Coordinates.X;
        float y = point.Coordinates.Y;
        drawer.DrawEllipse(DrawerPen, x - 1f, y - 1f, 2f, 2f);

        WriteMessage(name, x, y);
    }
    public void DrawRay(Ray ray, GString? name = null)
    {
        double x1 = ray.Point1.Coordinates.X;
        double y1 = ray.Point1.Coordinates.Y;
        double x2 = ray.Point2.Coordinates.X;
        double y2 = ray.Point2.Coordinates.Y;

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
    public void DrawSegment(Segment segment, GString? name = null)
    {
        float x1 = segment.Point1.Coordinates.X;
        float y1 = segment.Point1.Coordinates.Y;
        float x2 = segment.Point2.Coordinates.X;
        float y2 = segment.Point2.Coordinates.Y;
        drawer.DrawLine(DrawerPen, x1, y1, x2, y2);

        double xMessage = (x1 + x2) / 2;
        double yMessage = (y1 + y2) / 2;
        WriteMessage(name, xMessage, yMessage);
    }
    public void DrawSequence<T>(GSharpSequence<T> sequence) where T : GSObject, IDrawable
    {
        foreach (var obj in sequence.GenericSequence)
            obj.Draw(this, null);
    }
    public void DrawEnumerable(IEnumerable values)
    {
        try
        {
            foreach (var obj in values)
            {
                if(obj is not null)
                    ((IDrawable)obj).Draw(this, null);
            }
        }
        catch
        {
            throw new DefaultError("Object cannot be drawed");
        }
    }
    public void DrawString(GString gString)
    {
        Font font = new("Arial", 16);
        Brush brush = new SolidBrush(Color.Black);
        drawer.DrawString(gString.Value, font, brush, 0, 0);
        font.Dispose();
        brush.Dispose();
    }
    public void SetColor(string newColor)
    {
        usedColors.Push(DrawerPen.Color);
        DrawerPen.Color = Color.FromName(newColor);        
    }
    public void ResetColor()
    {
        if (usedColors.TryPop(out Color oldColor))
             DrawerPen.Color = oldColor;
    }
    public void Reset()
    {
        usedColors.Clear();
        DrawerPen.Color = Color.Black;
    }
}