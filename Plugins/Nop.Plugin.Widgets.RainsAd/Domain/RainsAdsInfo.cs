using Nop.Core;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.RainsAd.Domain
{
    /// <summary>
    ///ad info
    /// </summary>
    public partial class RainsAdsInfo : BaseEntity
    {
        public string Name { get; set; }

        public string Text { get; set; }
        public string WidgetZone { get; set; }
        public int PictureId { get; set; }
        /// <summary>
        /// Í¼Æ¬ÊıÁ¿
        /// </summary>
        public int PictureCount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public string Tilte1 { get; set; }
        public string Link1{ get; set; }
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

    }
}