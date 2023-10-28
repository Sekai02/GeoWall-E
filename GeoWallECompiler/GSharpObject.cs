namespace GeoWallECompiler;
public abstract class GSharpObject
{
    public abstract double ToValueOfTruth();
}
public class GSharpNumber : GSharpObject
{
    public GSharpNumber(double val) => Value = val;
    public override double ToValueOfTruth() => Value == 0 ? 0 : 1;
    public double Value { get; private set; }

    #region Operators Overloads
    public static GSharpNumber operator +(GSharpNumber a) => new(a.Value);
    public static GSharpNumber operator -(GSharpNumber a) => new(-a.Value);
    public static GSharpNumber operator +(GSharpNumber a, GSharpNumber b) => new(a.Value + b.Value);
    public static GSharpNumber operator -(GSharpNumber a, GSharpNumber b) => new(a.Value - b.Value);
    public static GSharpNumber operator *(GSharpNumber a, GSharpNumber b) => new(a.Value * b.Value);
    public static GSharpNumber operator /(GSharpNumber a, GSharpNumber b) => new(a.Value / b.Value);
    public static GSharpNumber operator %(GSharpNumber a, GSharpNumber b) => new(a.Value % b.Value);
    public static GSharpNumber operator <(GSharpNumber a, GSharpNumber b) => a.Value < b.Value? new(1): new(0);
    public static GSharpNumber operator >(GSharpNumber a, GSharpNumber b) => a.Value > b.Value ? new(1) : new(0);
    public static GSharpNumber operator <=(GSharpNumber a, GSharpNumber b) => a.Value <= b.Value ? new(1) : new(0);
    public static GSharpNumber operator >=(GSharpNumber a, GSharpNumber b) => a.Value >= b.Value ? new(1) : new(0);
    public static GSharpNumber operator ==(GSharpNumber a, GSharpNumber b) => a.Value == b.Value ? new(1) : new(0);
    public static GSharpNumber operator !=(GSharpNumber a, GSharpNumber b) => a.Value != b.Value ? new(1) : new(0);
    #endregion
}
public class GSharpString : GSharpObject
{
    public GSharpString(string val) => Value = val;
    public override double ToValueOfTruth() => Value == "" ? 0 : 1;
    public string Value { get; private set; }
}
public class GSharpPoint : GSharpObject, IDrawable
{
    public GSharpPoint(GSharpNumber? x, GSharpNumber? y)
    {
        if (x is not null && y is not null)
            Coordinates = (x, y);
        else
            Coordinates = null;
    }
    public override double ToValueOfTruth() => Coordinates is null? 0 : 1;
    public void Draw() => throw new NotImplementedException();
    public (GSharpNumber X, GSharpNumber Y)? Coordinates { get; private set; }
}
