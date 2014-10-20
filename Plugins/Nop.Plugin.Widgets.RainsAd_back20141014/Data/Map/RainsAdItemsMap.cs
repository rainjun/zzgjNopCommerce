using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.Widgets.RainsAd.Domain;

namespace Nop.Plugin.Widgets.RainsAd.Data
{
    public partial class RainsAdItemsMap : EntityTypeConfiguration<RainsAdItems>
    {
        public RainsAdItemsMap()
        {
            this.ToTable("RainsAdItems");
            this.HasKey(x => x.Id);

            this.HasRequired(raItem => raItem.RainsAds)
                .WithMany(ra => ra.RainsAdItems)
                .HasForeignKey(raItem => raItem.AId);
        }
    }
}