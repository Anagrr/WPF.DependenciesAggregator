using System.Windows;
using DependenciesAggregator.Abstractions.Interfaces;
using DependenciesAggregator.BusinessLogic;
using DependenciesAggregator.Integrations.Implementations;
using DependenciesAggregator.Integrations.Implementations.Nuget;
using DependenciesAggregator.Integrations.Interfaces;
using DependenciesAggregator.Integrations.Interfaces.Nuget;
using Microsoft.Extensions.DependencyInjection;

namespace DependenciesAggregator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            this.ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IAggregator, Aggregator>();
            services.AddSingleton<INugetFeedClient, NugetFeedClient>();
            services.AddScoped<ICsprojFileProcessor, CsprojFileProcessor>();
            services.AddScoped<ILocalSourceProvider, LocalSourceProvider>();
            services.AddScoped<IAzureDevOpsProvider, AzureDevOpsProvider>();
            services.AddSingleton<MainWindow>();
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}