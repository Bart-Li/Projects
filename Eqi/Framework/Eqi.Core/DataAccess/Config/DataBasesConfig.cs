using System.Collections.Generic;
using System.Xml.Serialization;

namespace Eqi.Core.DataAccess.Config
{
    [XmlRoot(ElementName = "DatabasesConfig")]
    public class DataBasesConfig
    {
        /// <summary>
        /// Gets or sets database groups.
        /// </summary>
        [XmlElement("dbGroup")]
        public List<DataBaseGroup> DatabaseGroups { get; set; }
    }
}
