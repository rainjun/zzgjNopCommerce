using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.Widgets.RainsAd.Domain;

namespace Nop.Plugin.Widgets.RainsAd.Data
{
    public partial class RainsAdsMap : EntityTypeConfiguration<RainsAds>
    {
        public RainsAdsMap()
        {
            this.ToTable("RainsAds");
            this.HasKey(x => x.Id);
        }
    }
}