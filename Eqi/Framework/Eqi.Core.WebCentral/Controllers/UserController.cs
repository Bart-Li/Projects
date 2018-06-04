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
using Eqi.Core.DataAccess;
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
            return "test";
        }
    }
}
