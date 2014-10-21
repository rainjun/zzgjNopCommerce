using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.RainsAd.Models
{
    public class RainsAdsListModel : BaseNopModel
    {
        public RainsAdsListModel()
        {
            AvailableWidgetZones = new List<SelectListItem>();
        }

        public IList<SelectListItem> AvailableWidgetZones;
        [NopResourceDisplayName("Plugins.Widgets.RainsAd.WidgetZone")]
        public int WidgetZoneID { get; set; }
    }
}