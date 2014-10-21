using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.Widgets.RainsAd.Domain;

namespace Nop.Plugin.Widgets.RainsAd.Data
{
    public partial class RainsAdsInfoMap : EntityTypeConfiguration<RainsAdsInfo>
    {
        public RainsAdsInfoMap()
        {
            this.ToTable("RainsAdsInfo");
            this.HasKey(x => x.Id);
        }
    }
}