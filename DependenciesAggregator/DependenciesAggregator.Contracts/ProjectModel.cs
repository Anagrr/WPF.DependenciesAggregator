using System.Collections.Generic;
using System.IO;

namespace DependenciesAggregator.Contracts
{
    public class ProjectModel
    {
        private string name;

        public string Name
        {
            get
            {
                var actualName = this.name;

                if (this.name.EndsWith(".csproj"))
                {
                    actualName = Path.GetFileNameWithoutExtension(this.name);
                }

                return actualName;
            }
            set => this.name = value;
        }

        public List<PackageModel> Packages { get; set; }

        public List<ProjectModel> Projects { get; set; }
    }
}