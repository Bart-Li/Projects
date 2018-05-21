using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Eqi.Core;
using Eqi.Core.Reflection;

namespace Eqi.Core.WebCentral.Controllers
{
    public class UserController : ApiController
    {
        private ICurrentAppDomain domain;

        public UserController()
        {
            domain = LibraryContainer.Get<ICurrentAppDomain>();
        }

        [HttpGet]
        public string Get()
        {
            return domain.BaseDirectory;
        }
    }
}
