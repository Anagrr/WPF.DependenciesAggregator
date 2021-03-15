using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using DependenciesAggregator.Abstractions.Interfaces;
using DependenciesAggregator.Contracts;

namespace DependenciesAggregator.BusinessLogic
{
    public class CsprojFileProcessor : ICsprojFileProcessor
    {
        private Func<XElement, bool> IsPackageDependecy = el =>
            el.Name.ToString().Equals("PackageReference") && el.Attribute("Include") != null;

        private Func<XElement, bool> IsProjectDependecy = el =>
                    el.Name.ToString().Equals("ProjectReference") && el.Attribute("Include") != null;


        public ProjectModel Process(string xmlFilePath)
        {
            var dependenciesQuery = XElement.Load(xmlFilePath).Elements()
                .Where(x => x.Name.ToString().Equals("ItemGroup"))
                .SelectMany(x => x.Elements());

            return new ProjectModel
            {
                Name = Path.GetFileNameWithoutExtension(xmlFilePath),
                Packages = dependenciesQuery.Where(el => this.IsPackageDependecy(el))
                    .Select(el => new PackageModel
                    {
                        Name = el.Attribute("Include").Value,
                        Version = this.GetVersionString(el)
                    })
                    .ToList(),
                Projects = dependenciesQuery.Where(el => this.IsProjectDependecy(el))
                    .Select(el => new ProjectModel { Name = el.Attribute("Include").Value })
                    .ToList()
            };
        }

        private string GetVersionString(XElement el)
        {
            var v = el.Attribute("Version")?.Value;
            return string.IsNullOrEmpty(v) ? string.Empty : $"v.{v}";
        }
    }
}