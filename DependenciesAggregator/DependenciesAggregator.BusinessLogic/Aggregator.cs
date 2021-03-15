using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DependenciesAggregator.Abstractions.Interfaces;
using DependenciesAggregator.Contracts;
using DependenciesAggregator.Integrations.Implementations;

namespace DependenciesAggregator.BusinessLogic
{
    public class Aggregator : IAggregator
    {
        private readonly ICsprojFileProcessor processor;

        public List<ProjectModel> Projects { get; } = new List<ProjectModel>();

        public Aggregator()
        {
            this.processor = new CsprojFileProcessor();
        }

        public void AggregateFromAzure(List<string> linksToRepositories)
        {
            this.Aggregate(new AzureDevOpsProvider(linksToRepositories).FetchData());
        }

        public void AggregateFromLocal(string rootDirectory)
        {
            this.Aggregate(new LocalSourceProvider(rootDirectory).FetchData());
        }

        private void Aggregate(IEnumerable<string> filesPathes)
        {
            this.Projects.Clear();

            foreach (var path in filesPathes)
            {
                var projectModel = this.processor.Process(path);

                if (!projectModel.Packages.Any() && !projectModel.Projects.Any())
                {
                    continue;
                }

                if (this.Projects.Any(p => p.Name.Equals(projectModel.Name)))
                {
                    projectModel.Name = path;
                }

                this.Projects.Add(projectModel);
            }
        }

        public void ExportToFile()
        {
        }
    }
}