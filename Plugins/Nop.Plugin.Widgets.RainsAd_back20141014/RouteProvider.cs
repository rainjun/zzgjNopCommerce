using Nop.Web.Framework.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Web.Mvc;


namespace Nop.Plugin.Widgets.RainsAd
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            //指定地址到conntroller
            routes.MapRoute("Plugin.Widgets.RainsAd.CreateAd",
                "Plugins/WidgetsRainsAd/CreateAd",
                new { controller = "WidgetsRainsAd", action = "CreateAd" },
                new[] { "Nop.Plugin.Widgets.RainsAd.Controllers" }
           );

            //指定地址到conntroller
            routes.MapRoute("Plugin.Widgets.RainsAd.RainsAdUpdate",
                "Plugins/WidgetsRainsAd/RainsAdUpdate",
                new { controller = "WidgetsRainsAd", action = "RainsAdUpdate" },
                new[] { "Nop.Plugin.Widgets.RainsAd.Controllers" }
           );

            //指定地址到conntroller
            routes.MapRoute("Plugin.Widgets.RainsAd.WidgetZones",
                "Plugins/WidgetsRainsAd/WidgetZones",
                new { controller = "WidgetsRainsAd", action = "WidgetZones" },
                new[] { "Nop.Plugin.Widgets.RainsAd.Controllers" }
           );

        }
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
