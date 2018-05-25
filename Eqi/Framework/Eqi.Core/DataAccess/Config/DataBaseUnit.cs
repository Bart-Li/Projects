using System.Xml.Serialization;

namespace Eqi.Core.DataAccess.Config
{
    /// <summary>
    /// Database unit.
    /// </summary>
    public class DataBaseUnit
    {
        /// <summary>
        /// Gets or sets database name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets database type.
        /// </summary>
        [XmlAttribute("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets connection string list.
        /// </summary>
        [XmlElement("connectionString")]
        public string ConnectionString { get; set; }
    }
}
