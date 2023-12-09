namespace GeoWallECompiler;
public interface IWalleUI
{
    public void Print(object? obj, string caption = "");
    public void PrintError(GSharpException obj);
    public void PrintErrors(List<GSharpException> exceptions)
    {
        foreach (var exception in exceptions)
            PrintError(exception);
    }
}
