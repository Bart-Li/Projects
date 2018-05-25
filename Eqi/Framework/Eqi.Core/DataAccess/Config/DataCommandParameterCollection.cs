using System.Collections.Generic;
using System.Xml.Serialization;

namespace Eqi.Core.DataAccess.Config
{
    /// <summary>
    /// Data command parameter collection.
    /// </summary>
    public class DataCommandParameterCollection
    {
        /// <summary>
        /// Gets or sets parameter list.
        /// </summary>
        [XmlElement("param")]
        public List<DataParameterUnit> ParameterCollection { get; set; }

        /// <summary>
        /// Gets or sets parameter group collection.
        /// </summary>
        [XmlElement("paramGroup")]
        public List<DataParameterGroupUnit> ParameterGroupCollection { get; set; }
    }
}
