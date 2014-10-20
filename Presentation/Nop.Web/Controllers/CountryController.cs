using System;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Web.Infrastructure.Cache;

namespace Nop.Web.Controllers
{
    public partial class CountryController : BasePublicController
    {
        #region Fields

        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Constructors

        public CountryController(ICountryService countryService,
            IStateProvinceService stateProvinceService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            ICacheManager cacheManager)
        {
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._cacheManager = cacheManager;
        }

        #endregion

        #region States / provinces

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetStatesByCountryId(string countryId, bool addEmptyStateIfRequired)
        {
            //this action method gets called via an ajax request
            if (String.IsNullOrEmpty(countryId))
                throw new ArgumentNullException("countryId");

            string cacheKey = string.Format(ModelCacheEventConsumer.STATEPROVINCES_BY_COUNTRY_MODEL_KEY, countryId, addEmptyStateIfRequired, _workContext.WorkingLanguage.Id);
            var cacheModel = _cacheManager.Get(cacheKey, () =>
            {
                var country = _countryService.GetCountryById(Convert.ToInt32(countryId));
                var states = _stateProvinceService.GetStateProvincesByCountryId(country != null ? country.Id : 0).ToList();
                var result = (from s in states
                              select new { id = s.Id.ToString(), name = s.GetLocalized(x => x.Name) })
                              .ToList();

                if (addEmptyStateIfRequired && result.Count == 0)
                    result.Insert(0, new { id = "-1", name = _localizationService.GetResource("Address.Select") });
                return result;

            });

            return Json(cacheModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// rainAdd
        /// 获取省份
        /// </summary>
        /// <param name="addEmptyStateIfRequired"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMyProvinces(bool addEmptyStateIfRequired)
        {
            string cacheKey = string.Format(ModelCacheEventConsumer.STATEPROVINCES_BY_COUNTRY_MODEL_KEY, "GetMyProvinces", addEmptyStateIfRequired, _workContext.WorkingLanguage.Id);
            var cacheModel = _cacheManager.Get(cacheKey, () =>
            {
                var states = _stateProvinceService.GetMyProvinces().ToList();
                var result = (from s in states
                              select new { id = s.Id.ToString(), name = s.GetLocalized(x => x.Name) })
                              .ToList();

                if (addEmptyStateIfRequired)
                    result.Insert(0, new { id = "-1", name = _localizationService.GetResource("Address.Select") });
                return result;

            });

            return Json(cacheModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// rainAdd
        /// 获取市区
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="addEmptyStateIfRequired"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAreaByParentId(string parentId, bool addEmptyStateIfRequired)
        {
            //this action method gets called via an ajax request
            if (String.IsNullOrEmpty(parentId)
                || "-1".Equals(parentId))
            {
                //throw new ArgumentNullException("parentId");
                var defaultSelect = new[] { new { id = "-1", name = _localizationService.GetResource("Address.Select") } };

                return Json(defaultSelect, JsonRequestBehavior.AllowGet);
            }

            string cacheKey = string.Format(ModelCacheEventConsumer.STATEPROVINCES_BY_COUNTRY_MODEL_KEY, parentId, addEmptyStateIfRequired, _workContext.WorkingLanguage.Id);
            var cacheModel = _cacheManager.Get(cacheKey, () =>
            {
                var parentArea = _stateProvinceService.GetStateProvinceById(Convert.ToInt32(parentId));
                var states = _stateProvinceService.GetStateProvincesByParendId(parentArea != null ? parentArea.Id : 0).ToList();
                var result = (from s in states
                              select new { id = s.Id.ToString(), name = s.GetLocalized(x => x.Name) })
                              .ToList();

                if (addEmptyStateIfRequired)
                    result.Insert(0, new { id = "-1", name = _localizationService.GetResource("Address.Select") });
                return result;

            });

            return Json(cacheModel, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
