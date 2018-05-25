using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Eqi.Core.DataAccess.Config
{
    public class DataBaseGroup
    {
        /// <summary>
        /// Gets or sets group name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets database instances.
        /// </summary>
        [XmlElement("database")]
        public List<DataBaseUnit> DatabaseInstances { get; set; }

        /// <summary>
        /// Gets database collection.
        /// </summary>
        [XmlIgnore]
        public IEnumerable<DataBaseUnit> DatabaseCollection
        {
            get { return this.DatabaseInstances ?? Enumerable.Empty<DataBaseUnit>(); }
        }
    }
}
