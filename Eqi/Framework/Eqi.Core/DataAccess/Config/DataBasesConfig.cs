using System.Collections.Generic;
using System.Xml.Serialization;
using Eqi.Core.Configuration;

namespace Eqi.Core.DataAccess.Config
{
    [XmlRoot(ElementName = "DatabasesConfig")]
    [ConfigFile("Data/Databases.config", Format = FileFormat.Xml)]
    public class DataBasesConfig
    {
        /// <summary>
        /// Gets or sets database groups.
        /// </summary>
        [XmlElement("dbGroup")]
        public List<DataBaseGroup> DatabaseGroups { get; set; }
    }
}
