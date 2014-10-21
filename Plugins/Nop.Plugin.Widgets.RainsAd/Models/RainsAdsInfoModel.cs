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
        [UIHint("Picture")]
        public int PictureId { get; set; }
        /// <summary>
        /// 图片数量
        /// </summary>
        [NopResourceDisplayName("Plugins.Widgets.RainsAds.Fields.PictureCount")]
        public int PictureCount { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.RainsAds.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.RainsAds.Fields.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }

        [AllowHtml]
        public string Tilte1 { get; set; }
        [AllowHtml]
        public string Link1 { get; set; }
        [UIHint("Picture")]
        public int PictureId1 { get; set; }
        [AllowHtml]
        public string Tilte2 { get; set; }
        [AllowHtml]
        public string Link2 { get; set; }
        [UIHint("Picture")]
        public int PictureId2 { get; set; }
        [AllowHtml]
        public string Tilte3 { get; set; }
        [AllowHtml]
        public string Link3 { get; set; }
        [UIHint("Picture")]
        public int PictureId3 { get; set; }
        [AllowHtml]
        public string Tilte4 { get; set; }
        [AllowHtml]
        public string Link4 { get; set; }
        [UIHint("Picture")]
        public int PictureId4 { get; set; }
        [AllowHtml]
        public string Tilte5 { get; set; }
        [AllowHtml]
        public string Link5 { get; set; }
        [UIHint("Picture")]
        public int PictureId5 { get; set; }
        [AllowHtml]
        public string Tilte6 { get; set; }
        [AllowHtml]
        public string Link6 { get; set; }
        [UIHint("Picture")]
        public int PictureId6 { get; set; }
        [AllowHtml]
        public string Tilte7 { get; set; }
        [AllowHtml]
        public string Link7 { get; set; }
        [UIHint("Picture")]
        public int PictureId7 { get; set; }

        public IList<RainsAdItemsModel> Items { get; set; }

        #region Nested Classes



        #endregion


    }
}