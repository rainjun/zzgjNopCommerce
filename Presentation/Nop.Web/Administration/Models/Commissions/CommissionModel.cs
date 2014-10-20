using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Admin.Models.Commissions
{
    public class CommissionModel : BaseNopEntityModel
    {
        public CommissionModel()
        {
        }


        //identifiers
        [NopResourceDisplayName("Admin.Commission.Fields.ID")]
        public override int Id { get; set; }
        [NopResourceDisplayName("Admin.Commission.Fields.OrderGuid")]
        public Guid OrderGuid { get; set; }
        //customer info
        [NopResourceDisplayName("Admin.Commission.Fields.Customer")]
        public int CustomerId { get; set; }

        [NopResourceDisplayName("Admin.Commission.Fields.Customer")]
        public string CustomerUserName { get; set; }

        [NopResourceDisplayName("Admin.Commission.Fields.ShippingAddress")]
        public int CusShippingAddressIdtomerId { get; set; }

        [NopResourceDisplayName("Admin.Commission.Fields.CommissionStatus")]
        public int CommissionStatusId { get; set; }
        [NopResourceDisplayName("Admin.Commission.Fields.CommissionStatus")]
        public string CommissionStatus { get; set; }

        [NopResourceDisplayName("Admin.Commission.Fields.OrdersTotal")]
        public string OrdersTotal { get; set; }

        [NopResourceDisplayName("Admin.Commission.Fields.CommissionsTotal")]
        public string CommissionsTotal { get; set; }

        [NopResourceDisplayName("Admin.Commission.Fields.AppliedDate")]
        public DateTime AppliedDate { get; set; }

        [NopResourceDisplayName("Admin.Commission.Fields.PaidDate")]
        public DateTime PaidDate { get; set; }

        [NopResourceDisplayName("Admin.Commission.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }


        //items
        public IList<CommissionItemModel> Items { get; set; }

        #region Nested Classes

        public partial class CommissionItemModel : BaseNopEntityModel
        {
            public CommissionItemModel()
            {
            }
            public int OrderItemId { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Ratio { get; set; }
            public decimal UnitCommission { get; set; }
            public decimal TotalCommission { get; set; }
        }

        #endregion
    }
}