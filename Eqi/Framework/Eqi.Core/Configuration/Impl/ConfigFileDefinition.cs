using System;

namespace Eqi.Core.Configuration.Impl
{
    [DependencyInjection(typeof(IConfigFileDefinition))]
    public class ConfigFileDefinition : IConfigFileDefinition
    {
        /// <summary>
        /// The name for matching ConfigFileAttribute["Name"].
        /// </summary>
        private string name;

        private FileFormat format = FileFormat.None;

        /// <summary>
        /// Gets the name.
        /// The name for matching ConfigFileAttribute["Name"].
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.name))
                {
                    return this.name;
                }

                var paths = FilePath
                    .Replace("\\", "@")
                    .Replace(@"\", "@")
                    .Replace("/", "@")
                    .Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);

                return paths[paths.Length - 1].Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
            set
            {
                this.name = value;
            }
        }

        public string FilePath { get; set; }

        public FileFormat Format
        {
            get
            {
                if (this.format != FileFormat.None)
                {
                    return this.format;
                }

                if (FilePath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    this.format = FileFormat.Xml;
                }
                else if (FilePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    this.format = FileFormat.Json;
                }
                else if (FilePath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    this.format = FileFormat.Text;
                }
                else if (FilePath.EndsWith(".ini", StringComparison.OrdinalIgnoreCase))
                {
                    this.format = FileFormat.Ini;
                }
                else
                {
                    this.format = FileFormat.Xml;
                }

                return this.format;
            }

            set
            {
                this.format = value;
            }
        }

        /// <summary>
        /// Returns a string that represents the current ConfigurationFile.
        /// </summary>
        /// <returns>A string that represents the current ConfigurationFile.</returns>
        public override string ToString()
        {
            return string.Format("ConfigurationName:[{0}],FilePath:[Configuration/{1}],Format:[{2}]", Name, FilePath, Format);
        }
    }
}
