using System;

namespace Eqi.Core.Configuration
{
    /// <summary>
    /// Configuration file attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigFileAttribute : Attribute
    {
        /// <summary>
        /// The options.
        /// </summary>
        private ConfigurationFile configFile;

        /// <summary>
        /// Initializes a new instance of the ConfigFileAttribute class.
        /// </summary>
        /// <param name="filePath">File path.</param>
        public ConfigFileAttribute(string filePath)
        {
            this.configFile = new ConfigurationFile();
            this.configFile.FilePath = filePath;
        }

        /// <summary>
        /// Gets or sets file path.
        /// </summary>
        public string FilePath
        {
            get { return this.configFile.FilePath; }
            set { this.configFile.FilePath = value; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return this.configFile.Name; }
            set { this.configFile.Name = value; }
        }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        public FileFormat Format
        {
            get { return this.configFile.Format.ToEnum<FileFormat>(FileFormat.Xml); }
            set { this.configFile.Format = value.ToString(); }
        }
    }
}
