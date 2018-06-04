using System.Collections.Generic;
using System.Xml.Serialization;
using Eqi.Core.Configuration;

namespace Eqi.Core.DataAccess.Config
{
    [XmlRoot(ElementName = "DataCommandFiles")]
    [ConfigFile("Data/DataCommandFiles.config", Format = FileFormat.Xml)]
    public class DataCommandFilesConfig
    {
        [XmlElement("file")]
        public List<DataCommandFile> DataCommandFiles { get; set; }
    }

    public class DataCommandFile
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
