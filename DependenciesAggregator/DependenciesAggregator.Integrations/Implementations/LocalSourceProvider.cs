using System.Collections.Generic;
using System.IO;
using DependenciesAggregator.Integrations.Interfaces;

namespace DependenciesAggregator.Integrations.Implementations
{
    public class LocalSourceProvider : ILocalSourceProvider
    {
        private string rootDir;
        
        public IEnumerable<string> FetchData()
        {
            return Directory.EnumerateFiles(this.rootDir, "*.csproj", SearchOption.AllDirectories);
        }

        public ILocalSourceProvider WithRootDir(string rootDir)
        {
            this.rootDir = rootDir;
            return this;
        }
    }
}