using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace DependenciesAggregator.Integrations.Interfaces.Nuget
{
    public interface INugetFeedClient
    {
        Task<IPackageSearchMetadata> GetPackageMetadata(string package, string version);
    }
}