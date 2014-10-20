using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.RainsAd.Models
{
    public partial class RainsAdItemsModel : BaseNopEntityModel
    {
        public RainsAdItemsModel()
        {
        }
        public int Id { get; set; }
        public int Aid { get; set; }
        public string Name { get; set; }
        [UIHint("Picture")]
        public int PictureId { get; set; }
        public string PictureUrl { get; set; }
        public string Link { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.RainsAds.Fields.CreateOn")]
        public DateTime CreateOn { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.RainsAds.Fields.UpdateOn")]
        public DateTime UpdateOn { get; set; }
    }
}
