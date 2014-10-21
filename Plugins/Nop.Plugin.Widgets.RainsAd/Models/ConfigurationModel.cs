using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.RainsAd.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public ConfigurationModel()
        {
            AvailablePictureCount = new List<SelectListItem>();
            AvailableWidgetZones = new List<SelectListItem>();
        }

        public IList<SelectListItem> AvailablePictureCount;
        [NopResourceDisplayName("Plugins.Widgets.RainsAd.PictureCount")]
        public int PictureCount { get; set; }

        public IList<SelectListItem> AvailableWidgetZones;
        [NopResourceDisplayName("Plugins.Widgets.RainsAd.WidgetZone")]
        public int WidgetZone { get; set; }

    }
}