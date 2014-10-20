using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System;

namespace Nop.Plugin.Widgets.RainsAd.Models
{
    public class RainsAdsInfoModel : BaseNopEntityModel
    {
        public RainsAdsInfoModel()
        {
            Items = new List<RainsAdItemsModel>();
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

        public string Tilte1 { get; set; }
        public string Link1 { get; set; }
        public int PictureId1 { get; set; }
        public string Tilte2 { get; set; }
        public string Link2 { get; set; }
        public int PictureId2 { get; set; }
        public string Tilte3 { get; set; }
        public string Link3 { get; set; }
        public int PictureId3 { get; set; }
        public string Tilte4 { get; set; }
        public string Link4 { get; set; }
        public int PictureId4 { get; set; }
        public string Tilte5 { get; set; }
        public string Link5 { get; set; }
        public int PictureId5 { get; set; }
        public string Tilte6 { get; set; }
        public string Link6 { get; set; }
        public int PictureId6 { get; set; }
        public string Tilte7 { get; set; }
        public string Link7 { get; set; }
        public int PictureId7 { get; set; }

        public IList<RainsAdItemsModel> Items { get; set; }

        #region Nested Classes



        #endregion


    }
}