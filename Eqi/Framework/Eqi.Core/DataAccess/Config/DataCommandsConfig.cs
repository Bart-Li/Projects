using System.Collections.Generic;
using System.Xml.Serialization;

namespace Eqi.Core.DataAccess.Config
{
    /// <summary>
    /// Gets or sets data commands config.
    /// </summary>
    [XmlRoot(ElementName = "DataCommandsConfig")]
    public class DataCommandsConfig
    {
        /// <summary>
        /// Gets or sets data command list.
        /// </summary>
        [XmlElement("dataCommand")]
        public List<DataCommandUnit> DataCommandCollection { get; set; }
    }
}
