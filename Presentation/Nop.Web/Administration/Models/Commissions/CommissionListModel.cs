using Nop.Admin.Models.Orders;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace Nop.Admin.Models.Commissions
{
    public class CommissionListModel : OrderListModel
    {
        public CommissionListModel()
            : base()
        {
        }


        //[NopResourceDisplayName("Admin.Orders.List.StartDate")]
        //[UIHint("DateNullable")]
        //public DateTime? StartDate { get; set; }

        //[NopResourceDisplayName("Admin.Orders.List.EndDate")]
        //[UIHint("DateNullable")]
        //public DateTime? EndDate { get; set; }


        //public string CustomerUserName { get; set; }


        //[NopResourceDisplayName("Admin.Orders.List.StateProvice")]
        //public int StateProvinceId { get; set; }
        //[NopResourceDisplayName("Admin.Orders.List.City")]
        //public int CityId { get; set; }
        //[NopResourceDisplayName("Admin.Orders.List.District")]
        //public int DistrictId { get; set; }


        //public bool IsLoggedInAsDistributor { get; set; }

        //public IList<SelectListItem> AvailableStates { get; set; }
        //public IList<SelectListItem> AvailableCities { get; set; }
        //public IList<SelectListItem> AvailableDistricts { get; set; }
    }
}