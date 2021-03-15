using System.Collections.Generic;

namespace DependenciesAggregator.Integrations.Interfaces
{
    public interface IProvider
    {
        IEnumerable<string> FetchData();
    }
}