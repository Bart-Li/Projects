using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Eqi.Core.DataAccess.Config
{
    public class DataCommandUnit
    {
        /// <summary>
        /// Gets or sets command name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets database name.
        /// </summary>
        [XmlAttribute("database")]
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets command type.
        /// </summary>
        [XmlAttribute("commandType")]
        public string CommandType { get; set; }

        /// <summary>
        /// Gets or sets time out.
        /// </summary>
        [XmlAttribute("timeOut")]
        public int TimeOut { get; set; }

        /// <summary>
        /// Gets or sets command text.
        /// </summary>
        [XmlIgnore]
        public string CommandText { get; set; }

        /// <summary>
        /// Gets or sets free block content.
        /// </summary>
        [XmlElement(ElementName = "commandText", Type = typeof(XmlCDataSection))]
        public XmlCDataSection CDataCmmandText
        {
            get
            {
                return (new XmlDocument()).CreateCDataSection(this.CommandText);
            }

            set
            {
                if (value != null && !string.IsNullOrWhiteSpace(value.Value))
                {
                    this.CommandText = value.Value.Trim();
                }
                else
                {
                    this.CommandText = string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets or sets parameters.
        /// </summary>
        [XmlElement("parameters")]
        public DataCommandParameterCollection Parameters { get; set; }

        /// <summary>
        /// Gets data parameter collection.
        /// </summary>
        public IEnumerable<DataParameterUnit> ParameterCollection
        {
            get
            {
                IEnumerable<DataParameterUnit> result = Enumerable.Empty<DataParameterUnit>();

                if (this.Parameters != null && 
                    this.Parameters.ParameterCollection != null && 
                    this.Parameters.ParameterCollection.Any())
                {
                    result = this.Parameters.ParameterCollection;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets data parameter group collection.
        /// </summary>
        public IEnumerable<DataParameterGroupUnit> ParameterGroupCollection
        {
            get
            {
                IEnumerable<DataParameterGroupUnit> result = Enumerable.Empty<DataParameterGroupUnit>();

                if (this.Parameters != null &&
                    this.Parameters.ParameterGroupCollection != null &&
                    this.Parameters.ParameterGroupCollection.Any())
                {
                    result = this.Parameters.ParameterGroupCollection;
                }

                return result;
            }
        }
    }
}
