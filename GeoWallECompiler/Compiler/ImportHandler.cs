using GeoWallECompiler.Visitors;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GeoWallECompiler;

public class Container
{    
    public Context<GSObject?, ICallable> Environment { get; set; }
    public Context<GSharpType, ICallable> TypeEnvironment { get; set; }
    public Context<bool, bool> ResolvingContext { get; }

    public Container(Context<GSObject?, ICallable> environment, Context<GSharpType, ICallable> typeEnvironment, Context<bool,bool> resolvingContext, bool IsNotJson = true)
    {
        Environment = new();
        Environment.EatContext(environment);
        TypeEnvironment =new();
        TypeEnvironment.EatContext(typeEnvironment);
        ResolvingContext = new();
        ResolvingContext.EatContext(resolvingContext);
    }
    [JsonConstructor]
    public Container(Context<GSObject?, ICallable> environment, Context<GSharpType, ICallable> typeEnvironment, Context<bool, bool> resolvingContext)
    {
        Environment = environment;
        TypeEnvironment = typeEnvironment;
        ResolvingContext = resolvingContext;
    }

}

public static class ImportHandler
{
    private static Container GetEnvironments(string source, IDrawer drawer, IWalleUI userInterface) => GSharp.RunLibrary(source, drawer, userInterface);

    public static void SaveLibrary(string source, string path, IDrawer drawer, IWalleUI userInterface)
    {
        Container environments = GetEnvironments(source, drawer, userInterface);
        try 
        {
            JsonHelper.SerializeObject(environments, source, path);
        }
        catch (Exception e) 
        {
            userInterface.PrintError(new DefaultError(e.Message));
        }
    }

    public static Container LoadLibrary(string source)
    {
        source = source[1..^1];
        return JsonHelper.DeserializeObject<Container>(source);
    }
}