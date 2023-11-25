namespace GeoWallECompiler;
public interface IWalleUI
{
    public Task<Queue<double>> GetUserParameters(string message);
    //public void PrintOutput(object output);
}
