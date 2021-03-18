using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DependenciesAggregator.Integrations.Interfaces.Nuget;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace DependenciesAggregator.Integrations.Implementations.Nuget
{
    public class NugetFeedClient: INugetFeedClient
    {
        private readonly ILogger logger = NullLogger.Instance;
        
        public NugetFeedClient()
        {
                
        }
        
        public async Task<IPackageSearchMetadata> GetPackageMetadata(string packageName, string version)
        {
            var cancellationToken = CancellationToken.None;

            var cache = new SourceCacheContext();
            var repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            var resource = await repository.GetResourceAsync<PackageMetadataResource>();

            var packages = await resource.GetMetadataAsync(
                packageName,
                false,
                false,
                cache,
                logger,
                cancellationToken);

            return packages.FirstOrDefault(p => p.Identity.Version.ToString().Equals(version));
        }
    }
}