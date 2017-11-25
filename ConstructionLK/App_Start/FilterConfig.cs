﻿using System.Web;
using System.Web.Mvc;

namespace ConstructionLK
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute()); //
            filters.Add(new AuthorizeAttribute()); //Global Authorization
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
