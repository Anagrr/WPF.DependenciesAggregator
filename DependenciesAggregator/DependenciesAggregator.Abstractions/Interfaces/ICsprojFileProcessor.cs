using System.Collections.Generic;
using DependenciesAggregator.Contracts;

namespace DependenciesAggregator.Abstractions.Interfaces
{
    public interface ICsprojFileProcessor
    {
        ProjectModel Process(string xmlFilePath);
    }
}