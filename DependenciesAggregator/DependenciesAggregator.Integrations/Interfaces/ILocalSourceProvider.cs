namespace DependenciesAggregator.Integrations.Interfaces
{
    public interface ILocalSourceProvider : IProvider
    {
        ILocalSourceProvider WithRootDir(string rootDir);
    }
}