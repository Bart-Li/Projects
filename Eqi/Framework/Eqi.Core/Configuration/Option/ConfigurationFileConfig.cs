using System.Collections.Generic;
using System.Xml.Serialization;

namespace Eqi.Core.Configuration
{
    /// <summary>
    /// Configuration files config.
    /// </summary>
    [XmlRoot("ConfigurationFiles")]
    public class ConfigurationFileConfig
    {
        [XmlElement("ConfigFile")]
        public List<ConfigurationFile> ConfigurationFiles { get; set; }
    }

    public class ConfigurationFile
    {
        /// <summary>
        /// Gets or sets file path.
        /// </summary>
        [XmlAttribute("filePath")]
        public string FilePath { get; set; }

        /// <summary>
        /// Gets the name.
        /// The name for matching ConfigFileAttribute["Name"].
        /// </summary>
        /// <value>The name.</value>
        [XmlAttribute("name")]
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        [XmlAttribute("format")]
        public string Format { get; set; }

        /// <summary>
        /// From the specified ConfigFileAttribute.
        /// </summary>
        /// <returns>The from.</returns>
        /// <param name="configFileAttribute">Config file attribute.</param>
        //public static ConfigurationFile From(ConfigFileAttribute configFileAttribute)
        //{
        //    return new ConfigurationFile()
        //    {
        //        Name = configFileAttribute.Name,
        //        FilePath = configFileAttribute.FilePath,
        //        Format = configFileAttribute.Format
        //    };
        //}

        /// <summary>
        /// From the specified name, filePath, source and format.
        /// </summary>
        /// <returns>The from.</returns>
        /// <param name="name">Name.</param>
        /// <param name="filePath">File path.</param>
        /// <param name="format">Format.</param>
        //public static ConfigurationFile From(string name, string filePath, FileFormat format = FileFormat.Xml)
        //{
        //    return new ConfigurationFile()
        //    {
        //        Name = name,
        //        FilePath = filePath,
        //        Format = format
        //    };
        //}
    }
}