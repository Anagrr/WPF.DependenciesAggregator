using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DependenciesAggregator.Abstractions.Interfaces;
using DependenciesAggregator.Contracts;

namespace DependenciesAggregator.BusinessLogic
{
    public class CsprojFileProcessor : ICsprojFileProcessor
    {
        private const string PackageReferenceTag = "PackageReference";
        private const string ProjectReferenceTag = "ProjectReference";
        private const string ItemGroupTag = "ItemGroup";
        private const string IncludeAttribute = "Include";

        private readonly Func<XElement, bool> IsPackageDependecy = el =>
            el.Name.ToString().Equals(PackageReferenceTag) && el.Attribute(IncludeAttribute) != null;

        private readonly Func<XElement, bool> IsProjectDependecy = el =>
                    el.Name.ToString().Equals(ProjectReferenceTag) && el.Attribute(IncludeAttribute) != null;


        public ProjectModel Process(string xmlFilePath)
        {
            var dependenciesQuery = XElement.Load(xmlFilePath).Elements()
                .Where(x => x.Name.ToString().Equals(ItemGroupTag))
                .SelectMany(x => x.Elements())
                .Where(el => this.IsPackageDependecy(el) || this.IsProjectDependecy(el))
                .AsQueryable();

            return new ProjectModel
            {
                Name = Path.GetFileNameWithoutExtension(xmlFilePath),
                Packages = dependenciesQuery.Where(el => this.IsPackageDependecy(el))
                    .Select(el => new PackageModel
                    {
                        Name = el.Attribute(IncludeAttribute).Value,
                        Version = this.GetVersion(el)
                    })
                    .ToList(),
                Projects = dependenciesQuery.Where(el => this.IsProjectDependecy(el))
                    .Select(el => new ProjectModel { Name = el.Attribute(IncludeAttribute).Value })
                    .ToList()
            };
        }

        private string GetVersion(XElement el)
        {
            return el.Attribute("Version")?.Value;
        }
    }
}