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
    public partial class Commission : BaseEntity
    {

        #region Properties

        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public Guid OrderGuid { get; set; }


        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the shipping address identifier
        /// </summary>
        public int? ShippingAddressId { get; set; }

        /// <summary>
        /// Gets or sets the commission status identifier
        /// </summary>
        public int CommissionStatusId { get; set; }

        /// <summary>
        /// Gets or sets the order total
        /// </summary>
        public decimal OrdersTotal { get; set; }

        /// <summary>
        /// Gets or sets the Commission total
        /// </summary>
        public decimal CommissionsTotal { get; set; }


        /// <summary>
        /// Gets or sets the Applied date and time
        /// </summary>
        public DateTime? AppliedDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the paid date and time
        /// </summary>
        public DateTime? PaidDateUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the date and time of order creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        #endregion

        #region Navigation properties

        /// <summary>
        /// Gets or sets the customer
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the shipping address
        /// </summary>
        public virtual Address ShippingAddress { get; set; }

        /// <summary>
        /// Gets or sets discount usage history
        /// </summary>
        public virtual ICollection<Order> Orders
        {
            get { return _orders ?? (_orders = new List<Order>()); }
            protected set { _orders = value; }
        }

        private ICollection<Order> _orders;

        /// <summary>
        /// Gets or sets order items
        /// </summary>
        public virtual ICollection<CommissionItem> CommissionItems
        {
            get { return _commissionItems ?? (_commissionItems = new List<CommissionItem>()); }
            protected set { _commissionItems = value; }
        }
        private ICollection<CommissionItem> _commissionItems;

        #endregion

        #region Custom properties

        /// <summary>
        /// Gets or sets the commission status
        /// </summary>
        public CommissionStatus CommissionStatus
        {
            get
            {
                return (CommissionStatus)this.CommissionStatusId;
            }
            set
            {
                this.CommissionStatusId = (int)value;
            }
        }

        #endregion
    }
}
