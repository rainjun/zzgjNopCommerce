using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Tax;
using Nop.Core.Domain.Commissions;
using Nop.Core.Domain.Orders;

namespace Nop.Core.Domain.Commissions
{
    /// <summary>
    /// Represents an order
    /// </summary>
    public partial class CommissionItem : BaseEntity
    {

        #region Properties

        /// <summary>
        /// Gets or sets the commission identifier
        /// </summary>
        public int CommissionId { get; set; }

        /// <summary>
        /// Gets or sets the orderitem id
        /// </summary>
        public int OrderItemId { get; set; }

        /// <summary>
        /// Gets or sets the unit price
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the commission ratio
        /// </summary>
        public decimal Ratio { get; set; }

        /// <summary>
        /// Gets or sets the unit commission=Porduct.CommissionRatio*orderItem.UnitPriceExclTax
        /// </summary>
        public decimal UnitCommission { get; set; }

        /// <summary>
        /// Gets or sets the product`s commission= UnitCommission*Quantity
        /// </summary>
        public decimal TotalCommission { get; set; }

        #endregion

        #region Navigation properties

        /// <summary>
        /// Gets or sets the customer
        /// </summary>
        public virtual Commission Commission { get; set; }

        /// <summary>
        /// Gets or sets the shipping address
        /// </summary>
        public virtual OrderItem OrderItem { get; set; }


        #endregion


    }
}
