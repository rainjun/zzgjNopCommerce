using Nop.Plugin.Widgets.RainsAd.Domain;
using System.Data.Entity.ModelConfiguration;

namespace Nop.Plugin.Widgets.RainsAd.Data
{
    public partial class RainsWidgetZonesMap : EntityTypeConfiguration<RainsWidgetZones>
    {
        public RainsWidgetZonesMap()
        {
            this.ToTable("RainsWidgetZones");
            this.HasKey(x => x.Id);
        }
    }
}