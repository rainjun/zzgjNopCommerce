using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Commissions;

namespace Nop.Data.Mapping.Commissions
{
    public partial class CommissionMap : EntityTypeConfiguration<Commission>
    {
        public CommissionMap()
        {
            this.ToTable("Commission");
            this.HasKey(o => o.Id);

            this.Property(o => o.OrdersTotal).HasPrecision(18, 4);
            this.Property(o => o.CommissionsTotal).HasPrecision(18, 4);

            //rainAdd  增加佣金的适配
            this.Ignore(o => o.CommissionStatus);

            this.HasRequired(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId);

            //code below is commented because it causes some issues on big databases - http://www.nopcommerce.com/boards/t/11126/bug-version-20-command-confirm-takes-several-minutes-using-big-databases.aspx
            //this.HasRequired(o => o.BillingAddress).WithOptional().Map(x => x.MapKey("BillingAddressId")).WillCascadeOnDelete(false);
            //this.HasOptional(o => o.ShippingAddress).WithOptionalDependent().Map(x => x.MapKey("ShippingAddressId")).WillCascadeOnDelete(false);
            this.HasOptional(o => o.ShippingAddress)
                .WithMany()
                .HasForeignKey(o => o.ShippingAddressId)
                .WillCascadeOnDelete(false);

            this.HasMany(cs => cs.Orders)
                .WithMany(o => o.Commissions)
                .Map(m => m.ToTable("Commission_Order_Mapping"));
        }
    }
}