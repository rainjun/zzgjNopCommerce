using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System;

namespace Nop.Plugin.Widgets.RainsAd.Models
{
    public class RainsAdsModel : BaseNopEntityModel
    {
        public RainsAdsModel()
        {
            Items = new List<RainsAdItemsModel>();
            AvailableWidgetZones = new List<SelectListItem>();
            AvailablePictureCount = new List<SelectListItem>();
        }
        [NopResourceDisplayName("Plugins.Widgets.RainsAds.Fields.Name")]
        public string Name { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.RainsAds.Fields.Text")]
        public string Text { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.RainsAds.Fields.WidgetZone")]
        public string WidgetZone { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.RainsAds.Fields.PictureId")]
        public int PictureId { get; set; }
        /// <summary>
        /// 图片数量
        /// </summary>
        [NopResourceDisplayName("Plugins.Widgets.RainsAds.Fields.PictureCount")]
        public int PictureCount { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.RainsAds.Fields.CreateOn")]
        public DateTime CreateOn { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.RainsAds.Fields.UpdateOn")]
        public DateTime UpdateOn { get; set; }

        public IList<RainsAdItemsModel> Items { get; set; }

        public IList<SelectListItem> AvailableWidgetZones;
        public IList<SelectListItem> AvailablePictureCount;

        #region Nested Classes



        #endregion


    }
}