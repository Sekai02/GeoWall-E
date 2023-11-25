namespace GeoWallECompiler.Objects;
public interface IUserParameter<T>
{
    public abstract static T GetInstanceFromParameters(Queue<double> parameters);
}