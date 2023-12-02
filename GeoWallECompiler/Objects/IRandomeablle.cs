namespace GeoWallECompiler;
public interface IRandomable<T> where T : GSharpObject
{
    public static abstract T GetRandomInstance(int limit = 500);
}