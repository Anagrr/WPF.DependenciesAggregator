using System.Collections.Generic;
using System.IO;
using DependenciesAggregator.Integrations.Interfaces;

namespace DependenciesAggregator.Integrations.Implementations
{
    public class AzureDevOpsProvider : IProvider
    {
        private const string TempFolder = "Temp";
        private readonly List<string> linksToRepositories;
        private readonly LocalSourceProvider localSourceProvider;

        public AzureDevOpsProvider(List<string> linksToRepositories)
        {
            this.linksToRepositories = linksToRepositories;
            var currentDirectory = Directory.GetCurrentDirectory();
            var rootDir = Path.Combine(currentDirectory, TempFolder);
            this.localSourceProvider = new LocalSourceProvider(rootDir);
        }

        public IEnumerable<string> FetchData()
        {
            // get files from Azure
            // save to temp directory

            return this.localSourceProvider.FetchData();
        }
    }
}