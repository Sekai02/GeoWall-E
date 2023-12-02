namespace GeoWallECompiler;
public interface IRandomable<T> where T : GSObject
{
    public static abstract T GetRandomInstance(int limit = 500);
}