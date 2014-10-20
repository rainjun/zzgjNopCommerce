using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Commissions;

namespace Nop.Data.Mapping.Commissions
{
    public partial class CommissionItemMap : EntityTypeConfiguration<CommissionItem>
    {
        public CommissionItemMap()
        {
            this.ToTable("CommissionItem");
            this.HasKey(o => o.Id);

            this.Property(o => o.UnitCommission).HasPrecision(18, 4);
            this.Property(o => o.TotalCommission).HasPrecision(18, 4);
            this.Property(o => o.UnitPrice).HasPrecision(18, 4);
            this.Property(o => o.Ratio).HasPrecision(18, 4);

            this.HasRequired(o => o.Commission)
                .WithMany()
                .HasForeignKey(o => o.CommissionId);

            //在表中有对应的字段，然后根据父类的list区更新子类
            this.HasRequired(ci => ci.Commission)
                .WithMany(c => c.CommissionItems)
                .HasForeignKey(ci => ci.CommissionId);

            this.HasRequired(o => o.OrderItem)
                .WithMany()
                .HasForeignKey(o => o.OrderItemId);

        }
    }
}