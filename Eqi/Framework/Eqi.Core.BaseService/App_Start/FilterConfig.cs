﻿using System.Web;
using System.Web.Mvc;

namespace Eqi.Core.BaseService
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
