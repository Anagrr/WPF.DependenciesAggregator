using System.Collections.Generic;

namespace DependenciesAggregator.Integrations.Interfaces
{
    public interface IAzureDevOpsProvider : IProvider
    {
        IAzureDevOpsProvider ForRepositories(List<string> linksToRepositories);
    }
}