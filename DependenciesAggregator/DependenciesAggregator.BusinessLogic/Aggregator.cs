using System;
using System.Collections.Generic;
using System.Linq;
using DependenciesAggregator.Abstractions.Interfaces;
using DependenciesAggregator.Contracts;
using DependenciesAggregator.Integrations.Interfaces;

namespace DependenciesAggregator.BusinessLogic
{
    public class Aggregator : IAggregator
    {
        private readonly ICsprojFileProcessor processor;
        private readonly ILocalSourceProvider localSourceProvider;
        private readonly IAzureDevOpsProvider azureDevOpsProvider;

        public List<ProjectModel> AllProjects { get; } = new List<ProjectModel>();
        
        public List<ProjectModel> FilterProjects(Func<ProjectModel, bool> filter)
        {
            return this.AllProjects.Where(filter).ToList();
        }

        public Aggregator(ICsprojFileProcessor processor, ILocalSourceProvider localSourceProvider, IAzureDevOpsProvider azureDevOpsProvider)
        {
            this.processor = processor;
            this.localSourceProvider = localSourceProvider;
            this.azureDevOpsProvider = azureDevOpsProvider;

        }

        public void AggregateFromAzure(List<string> linksToRepositories)
        {
            this.Aggregate(this.azureDevOpsProvider.ForRepositories(linksToRepositories).FetchData());
        }

        public void AggregateFromLocal(string rootDirectory)
        {
            this.Aggregate(localSourceProvider.WithRootDir(rootDirectory).FetchData());
        }

        private void Aggregate(IEnumerable<string> filesPathes)
        {
            this.AllProjects.Clear();

            foreach (var path in filesPathes)
            {
                var projectModel = this.processor.Process(path);

                if (!projectModel.Packages.Any() && !projectModel.Projects.Any())
                {
                    continue;
                }

                if (this.AllProjects.Any(p => p.Name.Equals(projectModel.Name)))
                {
                    projectModel.Name = path;
                }

                this.AllProjects.Add(projectModel);
            }
        }

        public void ExportToFile()
        {
        }
    }
}