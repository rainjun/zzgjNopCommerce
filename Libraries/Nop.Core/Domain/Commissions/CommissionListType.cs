using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Commissions
{
    /// <summary>
    /// Represents a Commission status enumeration
    /// </summary>
    public enum CommissionListType : int
    {
        /*
         *o => o.CommissionStatusId == (int)CommissionStatus.Pending
         *                && o.OrderStatusId == (int)OrderStatus.Complete
         *                && o.CreatedOnUtc < firstDayOfThisMonth
         */
        /// <summary>
        /// 佣金未结算，而且订单已经完成（已确认收货），而且是上月的创建的
        /// 
        /// </summary>
        Order = 10,
        /*
         * o => o.CommissionStatusId == (int)CommissionStatus.Pending
         *               && (o.OrderStatusId == (int)OrderStatus.Pending
         *                   || o.OrderStatusId == (int)OrderStatus.Processing
         *                   || (o.OrderStatusId == (int)OrderStatus.Complete
         *                   && o.CreatedOnUtc >= firstDayOfThisMonth))
         */
        /// <summary>
        /// 佣金未结算
        /// </summary>
        New = 20
    }
}
