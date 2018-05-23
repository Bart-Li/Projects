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
        [XmlElement("configFile")]
        public List<ConfigurationFile> ConfigFiles { get; set; }
    }

    public class ConfigurationFile
    {
        /// <summary>
        /// Gets or set file name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets file path.
        /// </summary>
        [XmlAttribute("filePath")]
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        [XmlAttribute("format")]
        public string Format { get; set; }
    }
}