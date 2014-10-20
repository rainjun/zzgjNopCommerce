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
        /// <summary>
        /// Pending
        /// </summary>
        Order = 10,
        /// <summary>
        /// Processing
        /// </summary>
        New = 20
    }
}
