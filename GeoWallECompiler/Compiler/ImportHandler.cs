using GeoWallECompiler.Visitors;

namespace GeoWallECompiler;

public class Container
{
    public Context<GSObject?, ICallable> Environment;
    public Context<GSharpType, ICallable> TypeEnvironment;
    public Container(Context<GSObject?, ICallable> environment, Context<GSharpType, ICallable> typeEnvironment)
    {
        Environment = environment;
        TypeEnvironment = typeEnvironment;
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
        return JsonHelper.DeserializeObject<Container>(source);
    }
}