using System.Data;
using System.Xml.Serialization;

namespace Eqi.Core.DataAccess.Config
{
    public class DataParameterGroupUnit
    {
        /// <summary>
        /// Gets or sets parameter name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets db type.
        /// </summary>
        [XmlAttribute("dbType")]
        public DbType DbType { get; set; }

        /// <summary>
        /// Gets or sets arameter size.
        /// </summary>
        [XmlAttribute("size")]
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets parameter precision.
        /// </summary>
        [XmlAttribute("precision")]
        public byte Precision { get; set; }

        /// <summary>
        /// Gets or sets parameter scale.
        /// </summary>
        [XmlAttribute("scale")]
        public byte Scale { get; set; }
    }
}
