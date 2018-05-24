using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Serialization;
using Eqi.Core;
using Eqi.Core.Configuration;
using Eqi.Core.Reflection;
using Eqi.Core.Serialization;

namespace Eqi.Core.WebCentral.Controllers
{
    public class UserController : ApiController
    {
        private IConfigurationManager configManager;
        private ISerializer serializer;
        private ICurrentAppDomain domain;

        public UserController()
        {
            configManager = LibraryContainer.Get<IConfigurationManager>();
            serializer = LibraryContainer.Get<ISerializer>();
            domain = LibraryContainer.Get<ICurrentAppDomain>();
        }

        [HttpGet]
        public object Get()
        {
            var filePath = Path.Combine(this.domain.BaseDirectory, "Configuration/test2.txt");
            var configFile = configManager.GetConfiguration<string>(filePath);

            //var testClass = new Test { Name = "BartLi", Age = 23 };
            //var xml = serializer.SerializeToXml<Test>(testClass);
            //var configFile = serializer.DeserializeTextFromFile(filePath);
            //var configFile2 = serializer.DeserializeXmlFromFile<ConfigurationFileConfig>(filePath);
            return configFile;
        }

        public class Test
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }
    }
}
