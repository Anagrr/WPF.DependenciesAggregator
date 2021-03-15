using System.Collections.Generic;
using DependenciesAggregator.Contracts;

namespace DependenciesAggregator.Abstractions.Interfaces
{
    public interface IAggregator
    {
        void AggregateFromAzure(List<string> linksToRepositories);
        
        void AggregateFromLocal(string rootDirectory);

        void ExportToFile();

        List<ProjectModel> Projects { get; }
    }
}