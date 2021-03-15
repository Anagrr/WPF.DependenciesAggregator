using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DependenciesAggregator.Abstractions.Interfaces;
using DependenciesAggregator.BusinessLogic;
using DependenciesAggregator.Contracts;

namespace DependenciesAggregator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IAggregator aggregator;

        public MainWindow()
        {
            this.aggregator = new Aggregator();
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

            var packs = this.aggregator.Projects
                .Where(p => p.Name.Contains(packName, StringComparison.OrdinalIgnoreCase))
                .Where(p => p.Packages.Any(d => d.Name.Contains(dependencyName, StringComparison.OrdinalIgnoreCase))
                         || p.Projects.Any(d => d.Name.Contains(dependencyName, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            this.Draw(packs);
        }

        private void DrawAll()
        {
            this.Draw(this.aggregator.Projects);
        }

        private void Draw(List<ProjectModel> projects)
        {
            this.TreeView.ItemsSource = projects;
        }
    }
}
