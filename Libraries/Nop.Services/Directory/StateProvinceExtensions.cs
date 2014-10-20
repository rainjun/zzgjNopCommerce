using Nop.Core.Domain.Directory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Directory
{
    /// <summary>
    /// IStateProvinceService 扩展方法
    /// czhcom@163.com 2012年8月2日16:40:32
    /// </summary>
    public static class StateProvinceExtensions
    {
        /// <summary>
        /// Get formatted stateProvince breadcrumb 
        /// 省市区县面包屑呈现
        /// Note: ACL and store mapping is ignored
        /// </summary>
        /// <param name="category">stateProvince</param>
        /// <param name="categoryService">stateProvince service</param>
        /// <param name="separator">Separator</param>
        /// <returns>Formatted breadcrumb</returns>
        public static string GetFormattedBreadCrumb(this StateProvince stateProvince,
            IStateProvinceService stateProvinceService,
            string separator = ">>")
        {
            if (stateProvince == null)
                throw new ArgumentNullException("stateProvince");

            string result = string.Empty;

            //used to prevent circular references
            var alreadyProcessedstateProvinces = new List<int>() { };

            while (stateProvince != null &&  //not null
                !alreadyProcessedstateProvinces.Contains(stateProvince.Id)) //prevent circular references
            {
                if (String.IsNullOrEmpty(result))
                {
                    result = stateProvince.Name;
                }
                else
                {
                    result = string.Format("{0} {1} {2}", stateProvince.Name, separator, result);
                }

                alreadyProcessedstateProvinces.Add(stateProvince.Id);

                stateProvince = stateProvinceService.GetStateProvinceById(stateProvince.ParentId);

            }
            return result;
        }

        /// <summary>
        /// Get formatted StateProvinces breadcrumb 
        /// Note: ACL and store mapping is ignored
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="allCategories">All categories</param>
        /// <param name="separator">Separator</param>
        /// <returns>Formatted breadcrumb</returns>
        public static string GetFormattedBreadCrumb(this StateProvince stateProvince,
            IList<StateProvince> allStateProvinces,
            string separator = ">>")
        {
            if (stateProvince == null)
                throw new ArgumentNullException("stateProvince");

            if (allStateProvinces == null)
                throw new ArgumentNullException("stateProvince");

            string result = string.Empty;

            //used to prevent circular references 
            var alreadyProcessedstateProvinces = new List<int>() { };

            while (stateProvince != null && //not null 
                   !alreadyProcessedstateProvinces.Contains(stateProvince.Id)) //prevent circular references 
            {
                if (String.IsNullOrEmpty(result))
                {
                    result = stateProvince.Name;
                }
                else
                {
                    result = string.Format("{0} {1} {2}", stateProvince.Name, separator, result);
                }

                alreadyProcessedstateProvinces.Add(stateProvince.Id);

                stateProvince = (from c in allStateProvinces
                                 where c.Id == stateProvince.ParentId
                                 select c).FirstOrDefault();
            }
            return result;
        }
    }

}
