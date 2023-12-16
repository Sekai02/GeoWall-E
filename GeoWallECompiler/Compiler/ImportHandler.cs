using GeoWallECompiler.Visitors;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GeoWallECompiler;

public class Container
{
    public Context<GSObject?, ICallable> Environment { get; set; }
    public Context<GSharpType, ICallable> TypeEnvironment { get; set; }
    public Context<bool, bool> ResolvingContext { get; }
    public Dictionary<GSharpExpression, int> References { get; set; }

    public Container(Context<GSObject?, ICallable> environment, Context<GSharpType, ICallable> typeEnvironment, Context<bool, bool> resolvingContext, Dictionary<GSharpExpression, int> references)
    {
        Environment = new();
        Environment.EatContext(environment);
        TypeEnvironment = new();
        TypeEnvironment.EatContext(typeEnvironment);
        ResolvingContext = new();
        ResolvingContext.EatContext(resolvingContext);
        References = references;
    }

}

public static class ImportHandler
{
    private static Container? GetEnvironments(string source, IDrawer drawer, IWalleUI userInterface) => GSharp.RunLibraryFile(source, drawer, userInterface);
    public static Container? LoadLibrary(string source, IDrawer drawer, IWalleUI userInterface) => GetEnvironments(source, drawer, userInterface);
}