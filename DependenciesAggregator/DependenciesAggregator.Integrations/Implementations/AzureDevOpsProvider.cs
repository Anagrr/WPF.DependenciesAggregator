using System.Collections.Generic;
using System.IO;
using DependenciesAggregator.Integrations.Interfaces;

namespace DependenciesAggregator.Integrations.Implementations
{
    public class AzureDevOpsProvider : IAzureDevOpsProvider
    {
        private const string TempFolder = "Temp";
        private readonly ILocalSourceProvider localSourceProvider;
        private List<string> linksToRepositories;

        public AzureDevOpsProvider(ILocalSourceProvider localSourceProvider)
        {
            this.localSourceProvider = localSourceProvider;
        }

        public IEnumerable<string> FetchData()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var rootDir = Path.Combine(currentDirectory, TempFolder);
            this.localSourceProvider.WithRootDir(rootDir);

            // TODO: get files from Azure
            // TODO: save to temp directory

            return this.localSourceProvider.FetchData();
        }

        public IAzureDevOpsProvider ForRepositories(List<string> linksToRepositories)
        {
            this.linksToRepositories = linksToRepositories;
            return this;
        }
    }
}