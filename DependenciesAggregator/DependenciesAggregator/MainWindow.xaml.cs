using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DependenciesAggregator.Abstractions.Interfaces;
using DependenciesAggregator.Contracts;
using DependenciesAggregator.Integrations.Interfaces.Nuget;

namespace DependenciesAggregator
{
    public partial class MainWindow : Window
    {
        private readonly IAggregator aggregator;
        private readonly INugetFeedClient nugetFeedClient;
        private readonly List<ProjectModel> projects = new List<ProjectModel>();
        
        public MainWindow(IAggregator aggregator, INugetFeedClient nugetFeedClient)
        {
            this.aggregator = aggregator;
            this.nugetFeedClient = nugetFeedClient;

            InitializeComponent();
            this.RootDirPathInput.Text = @"C:\\Projects";
        }

        private void Aggregate_Click(object sender, RoutedEventArgs e)
        {
            this.aggregator.AggregateFromLocal(this.RootDirPathInput.Text);
            this.DrawAll();
        }

        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            var packName = this.PackageNameInput.Text;
            var dependencyName = this.DependencyNameInput.Text;
            
            if (string.IsNullOrEmpty(packName+dependencyName))
            {
                return;
            }

            var packs = this.aggregator.FilterProjects(
                    p => p.Name.Contains(packName, StringComparison.OrdinalIgnoreCase)
                     && (p.Packages.Any(d => d.Name.Contains(dependencyName, StringComparison.OrdinalIgnoreCase))
                        || p.Projects.Any(d => d.Name.Contains(dependencyName, StringComparison.OrdinalIgnoreCase)))).ToList();
            
            this.Draw(packs);
        }

        private void DrawAll()
        {
            this.Draw(this.aggregator.AllProjects);
        }

        private void Draw(IEnumerable<ProjectModel> items)
        {
            this.projects.Clear();
            this.projects.AddRange(items);
            this.ProjectsList.ItemsSource = this.projects.Select(p => p.Name);
            this.ProjectsList.SelectedIndex = 0;
        }


        private void ProjectsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ProjectsList.SelectedIndex < 0)
            {
                return;
            }

            var selectedProj = this.projects[this.ProjectsList.SelectedIndex];
            this.PackageDependencies.ItemsSource = selectedProj.Packages;
            this.ProjectDependencies.ItemsSource = selectedProj.Projects;
        }

        private void PackageDepsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ProjectsList.SelectedIndex < 0 || this.PackageDependencies.SelectedIndex < 0)
            {
                return;
            }

            var selectedPackage = this.projects[this.ProjectsList.SelectedIndex].Packages[this.PackageDependencies.SelectedIndex];
            // todo

            Task.Run(() => this.nugetFeedClient.GetPackageMetadata(selectedPackage.Name, selectedPackage.Version))
                .ContinueWith(task =>
                {
                    var meta = task.Result;
                });
            
        }
    }
}
