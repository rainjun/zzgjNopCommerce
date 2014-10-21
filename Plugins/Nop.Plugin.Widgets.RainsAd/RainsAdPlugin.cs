using System.Collections.Generic;
using System.IO;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Plugin.Widgets.RainsAd.Services;
using System;
using Nop.Plugin.Widgets.RainsAd.Data;
using Nop.Plugin.Widgets.RainsAd.Domain;

namespace Nop.Plugin.Widgets.RainsAd
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class RainsAdPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IRainsAdService _rainsAdService;
        private readonly RainsAdObjectContext _objectContext;

        public RainsAdPlugin(IPictureService pictureService,
            ISettingService settingService, IWebHelper webHelper, IRainsAdService rainsAdService, RainsAdObjectContext objectContext)
        {
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._webHelper = webHelper;
            this._rainsAdService = rainsAdService;
            this._objectContext = objectContext;
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            var allwidgetzones = _rainsAdService.GetWidgetZons();

            return allwidgetzones;
        }

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "WidgetsRainsAd";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Widgets.RainsAd.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for displaying widget
        /// </summary>
        /// <param name="widgetZone">Widget zone where it's displayed</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "WidgetsRainsAd";
            routeValues = new RouteValueDictionary()
            {
                {"Namespaces", "Nop.Plugin.Widgets.RainsAd.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            ////pictures
            //var sampleImagesPath = _webHelper.MapPath("~/Plugins/Widgets.RainsAd/Content/RainsAd/sample-images/");


            ////settings
            //var settings = new RainsAdSettings()
            //{
            //    Picture1Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner1.jpg"), "image/pjpeg", "banner_1", true).Id,
            //    Text1 = "",
            //    Link1 = _webHelper.GetStoreLocation(false),
            //    Picture2Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner2.jpg"), "image/pjpeg", "banner_2", true).Id,
            //    Text2 = "",
            //    Link2 = _webHelper.GetStoreLocation(false),
            //    Picture3Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner3.jpg"), "image/pjpeg", "banner_3", true).Id,
            //    Text3 = "",
            //    Link3 = _webHelper.GetStoreLocation(false),
            //    //Picture4Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner4.jpg"), "image/pjpeg", "banner_4", true).Id,
            //    //Text4 = "",
            //    //Link4 = _webHelper.GetStoreLocation(false),
            //};
            //_settingService.SaveSetting(settings);


            //this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.RainsAd.Picture1", "Picture 1");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.RainsAd.Picture2", "Picture 2");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.RainsAd.Picture3", "Picture 3");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.RainsAd.Picture4", "Picture 4");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.RainsAd.Picture5", "Picture 5");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.RainsAd.Picture", "Picture");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.RainsAd.Picture.Hint", "Upload picture.");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.RainsAd.Text", "Comment");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.RainsAd.Text.Hint", "Enter comment for picture. Leave empty if you don't want to display any text.");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.RainsAd.Link", "URL");
            //this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.RainsAd.Link.Hint", "Enter URL. Leave empty if you don't want this picture to be clickable.");
            //datas
            _objectContext.Install();

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<RainsAdSettings>();

            ////locales
            //this.DeletePluginLocaleResource("Plugins.Widgets.RainsAd.Picture1");
            //this.DeletePluginLocaleResource("Plugins.Widgets.RainsAd.Picture2");
            //this.DeletePluginLocaleResource("Plugins.Widgets.RainsAd.Picture3");
            //this.DeletePluginLocaleResource("Plugins.Widgets.RainsAd.Picture4");
            //this.DeletePluginLocaleResource("Plugins.Widgets.RainsAd.Picture5");
            //this.DeletePluginLocaleResource("Plugins.Widgets.RainsAd.Picture");
            //this.DeletePluginLocaleResource("Plugins.Widgets.RainsAd.Picture.Hint");
            //this.DeletePluginLocaleResource("Plugins.Widgets.RainsAd.Text");
            //this.DeletePluginLocaleResource("Plugins.Widgets.RainsAd.Text.Hint");
            //this.DeletePluginLocaleResource("Plugins.Widgets.RainsAd.Link");
            //this.DeletePluginLocaleResource("Plugins.Widgets.RainsAd.Link.Hint");

            //datas
            _objectContext.Uninstall();

            base.Uninstall();
        }
    }
}
