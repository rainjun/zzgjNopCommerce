using Nop.Core;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.RainsAd.Domain
{
    /// <summary>
    /// 广告的具体事项
    /// </summary>
    public partial class RainsAdItems : BaseEntity
    {
        public int AId { get; set; }
        public string Name { get; set; }
        public int PictureId { get; set; }

        public string Text { get; set; }
        public string Link { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime UpdateOn { get; set; }


        /// <summary>
        /// Gets the RainsAds
        /// </summary>
        public virtual RainsAds RainsAds { get; set; }

    }
}