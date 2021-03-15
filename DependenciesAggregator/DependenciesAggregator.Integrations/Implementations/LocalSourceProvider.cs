using System.Collections.Generic;
using System.IO;
using DependenciesAggregator.Integrations.Interfaces;

namespace DependenciesAggregator.Integrations.Implementations
{
    public class LocalSourceProvider : IProvider
    {
        private readonly string rootDir;
        public LocalSourceProvider(string rootDir)
        {
            this.rootDir = rootDir;
        }

        public IEnumerable<string> FetchData()
        {
            return Directory.EnumerateFiles(this.rootDir, "*.csproj", SearchOption.AllDirectories);
        }
    }
}