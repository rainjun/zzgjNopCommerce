using Nop.Core;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.RainsAd.Domain
{
    /// <summary>
    ///ad info
    /// </summary>
    public partial class RainsAds : BaseEntity
    {
        private ICollection<RainsAdItems> _rainsAdItems;

        public string Name { get; set; }

        public string Text { get; set; }
        public string WidgetZone { get; set; }
        public int PictureId { get; set; }
        /// <summary>
        /// Í¼Æ¬ÊýÁ¿
        /// </summary>
        public int PictureCount { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime UpdateOn { get; set; }

        /// <summary>
        /// Gets or sets the collection of applied discounts
        /// </summary>
        public virtual ICollection<RainsAdItems> RainsAdItems
        {
            get { return _rainsAdItems ?? (_rainsAdItems = new List<RainsAdItems>()); }
            protected set { _rainsAdItems = value; }
        }
    }
}