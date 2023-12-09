using System.Dynamic;

namespace GeoWallECompiler;

public static class ErrorHandler
{
    public static int Count { get => errors.Count; }
    public static bool HadError { get => Count != 0; }
    private static List<GSharpException> errors = new();
    public static void AddError(GSharpException error) => errors.Add(error);
    public static void Reset() => errors.Clear();
    public static List<GSharpException> GetErrors() => new List<GSharpException>(errors);
}