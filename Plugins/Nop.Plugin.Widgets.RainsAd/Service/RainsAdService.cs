using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Plugin.Widgets.RainsAd.Domain;
using Nop.Plugin.Widgets.RainsAd.Infrastructure.Cache;
using Nop.Services.Media;

namespace Nop.Plugin.Widgets.RainsAd.Services
{
    public partial class RainsAdService : IRainsAdService
    {
        #region Fields

        private readonly IRepository<RainsAds> _rainAdRepository;
        private readonly IRepository<RainsAdItems> _rainAdItemRepository;
        private readonly IRepository<RainsWidgetZones> _rainWidgetRepository;
        private readonly IRepository<RainsAdsInfo> _rainAdInfoRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IPictureService _pictureService;


        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string RAINSAD_PATTERN_KEY = "Nop.Widgets.RainsAd";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : rainsadinfo ID
        /// </remarks>
        private const string RAINSAD_INFO_BY_ID_KEY = "Nop.Widgets.RainsAd.RainsAdinfo.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : rainsadinfo ID
        /// </remarks>
        private const string RAINSAD_INFO_BY_WIDGETZONE_KEY = "Nop.Widgets.RainsAd.RainsAdinfo.widgetzone-{0}";


        /// <summary>
        /// Key for GetChildCategoryIds method results caching
        /// </summary>
        /// <remarks>
        /// {0} : parent category id
        /// {2} : comma separated list of customer roles
        /// {3} : current store ID
        /// </remarks>
        private const string RAINSAD_ALL_WIDGETZONES_KEY = "Nop.Widgets.RainsAd.WidgetZones";

        #endregion

        #region Ctor

        public RainsAdService(IRepository<RainsAds> rainAdRepository
            , IRepository<RainsAdItems> rainAdItemRepository
            , IRepository<RainsWidgetZones> rainWidgetRepository
            , IRepository<RainsAdsInfo> _rainAdInfoRepository
            , ICacheManager cacheManager
            , IPictureService pictureService)
        {
            this._rainAdRepository = rainAdRepository;
            this._rainAdItemRepository = rainAdItemRepository;
            this._rainWidgetRepository = rainWidgetRepository;
            this._rainAdInfoRepository = _rainAdInfoRepository;
            this._cacheManager = cacheManager;
            this._pictureService = pictureService;
        }

        #endregion

        #region Utilties
        //对输出的图片做缓存
        protected string GetPictureUrl(int pictureId)
        {
            string cacheKey = string.Format(ModelCacheEventConsumer.PICTURE_URL_MODEL_KEY, pictureId);
            return _cacheManager.Get(cacheKey, () =>
            {
                var url = _pictureService.GetPictureUrl(pictureId, showDefaultPicture: false);
                //little hack here. nulls aren't cacheable so set it to ""
                if (url == null)
                    url = "";

                return url;
            });
        }

        #endregion

        #region Methods

        public IPagedList<RainsAds> GetAllAds(int widgetZone = 0, string widgetZoneName = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _rainAdRepository.Table;

            if (widgetZone > 0)
            {
                var widgetZoneNameById = GetWidgetZoneById(widgetZone);
                if (null != widgetZoneNameById && !String.IsNullOrEmpty(widgetZoneNameById.Name))
                    query = query.Where(ad => ad.WidgetZone == widgetZoneNameById.Name);
            }
            if (!String.IsNullOrEmpty(widgetZoneName))
            {
                query = query.Where(ad => ad.WidgetZone.Equals(widgetZoneName));
            }

            query = query.OrderByDescending(ad => ad.Id);

            return new PagedList<RainsAds>(query, pageIndex, pageSize);
        }

        public void DeleteRainsAd(RainsAds rainsAd)
        {
            if (rainsAd == null)
                throw new ArgumentNullException("rainsAd");

            _rainAdRepository.Delete(rainsAd);
        }

        public void InsertRainsAd(RainsAds rainsAd)
        {
            if (rainsAd == null)
                throw new ArgumentNullException("rainsAd");

            _rainAdRepository.Insert(rainsAd);
        }

        public void UpdateRainsAd(RainsAds rainsAd)
        {
            if (rainsAd == null)
                throw new ArgumentNullException("rainsAd");

            _rainAdRepository.Update(rainsAd);
        }

        public RainsAds GetAdById(int rainsAdId)
        {
            if (rainsAdId == 0)
                return null;

            return _rainAdRepository.GetById(rainsAdId);
        }

        public IList<RainsAdItems> GetAdsItemsByAId(int aId)
        {
            var query = from gp in _rainAdItemRepository.Table
                        where gp.AId == aId
                        orderby gp.Id
                        select gp;
            var records = query.ToList();
            return records;
        }

        public RainsAdItems GetAdItemById(int rainsAdItemId)
        {
            if (rainsAdItemId == 0)
                return null;

            return _rainAdItemRepository.GetById(rainsAdItemId);
        }

        public void DeleteRainsAdItem(RainsAdItems rainsAdItems)
        {
            if (rainsAdItems == null)
                throw new ArgumentNullException("rainsAdItems");

            _rainAdItemRepository.Delete(rainsAdItems);
        }

        public void InsertRainsAdItem(RainsAdItems rainsAdItems)
        {
            if (rainsAdItems == null)
                throw new ArgumentNullException("rainsAdItems");

            _rainAdItemRepository.Insert(rainsAdItems);
        }

        public void UpdateRainsAdItem(RainsAdItems rainsAdItems)
        {
            if (rainsAdItems == null)
                throw new ArgumentNullException("rainsAdItems");

            _rainAdItemRepository.Update(rainsAdItems);
        }

        public IList<RainsWidgetZones> GetAllWidgetZons(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from gp in _rainWidgetRepository.Table
                        orderby gp.Id
                        select gp;
            return new PagedList<RainsWidgetZones>(query, pageIndex, pageSize);
        }


        public RainsWidgetZones GetWidgetZoneById(int widgetZoneId)
        {
            if (widgetZoneId == 0)
                return null;

            return _rainWidgetRepository.GetById(widgetZoneId);
        }

        public void DeleteRainsWidgetZone(RainsWidgetZones rainsWidgetZones)
        {
            if (rainsWidgetZones == null)
                throw new ArgumentNullException("rainsWidgetZones");

            _rainWidgetRepository.Delete(rainsWidgetZones);
        }

        public void InsertRainsWidgetZone(RainsWidgetZones rainsWidgetZones)
        {
            if (rainsWidgetZones == null)
                throw new ArgumentNullException("rainsWidgetZones");

            _rainWidgetRepository.Insert(rainsWidgetZones);
        }

        public void UpdateRainsWidgetZone(RainsWidgetZones rainsWidgetZones)
        {
            if (rainsWidgetZones == null)
                throw new ArgumentNullException("rainsWidgetZones");

            _rainWidgetRepository.Update(rainsWidgetZones);
        }

        /// <summary>
        /// 获取所有的定义的widgetZons
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<string> GetWidgetZons()
        {
            string key = RAINSAD_ALL_WIDGETZONES_KEY;
            return _cacheManager.Get(key, () =>
            {
                var allAdsInfo = GetAllRainsAdsInfo();
                List<string> wgzResult = new List<string>();
                foreach (var adInfo in allAdsInfo)
                {
                    if (!String.IsNullOrEmpty(adInfo.WidgetZone))
                    {
                        wgzResult.Add(adInfo.WidgetZone);
                    }
                }
                if (null != wgzResult && 0 < wgzResult.Count)
                {
                    return wgzResult;
                }
                else
                {
                    return null;
                }
            });
        }

        public IPagedList<RainsAdsInfo> GetAllRainsAdsInfo(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _rainAdInfoRepository.Table;

            query = query.OrderByDescending(ad => ad.Id);

            return new PagedList<RainsAdsInfo>(query, pageIndex, pageSize);
        }

        public void DeleteRainsAdInfo(RainsAdsInfo rainsAdInfo)
        {
            if (rainsAdInfo == null)
                throw new ArgumentNullException("rainsAdInfo");

            _rainAdInfoRepository.Delete(rainsAdInfo);

            //cache
            _cacheManager.RemoveByPattern(RAINSAD_PATTERN_KEY);
        }

        public void InsertRainsAdInfo(RainsAdsInfo rainsAdInfo)
        {
            if (rainsAdInfo == null)
                throw new ArgumentNullException("rainsAdInfo");

            _rainAdInfoRepository.Insert(rainsAdInfo);

            //cache
            _cacheManager.RemoveByPattern(RAINSAD_PATTERN_KEY);
        }

        public void UpdateRainsAdInfo(RainsAdsInfo rainsAdInfo)
        {
            if (rainsAdInfo == null)
                throw new ArgumentNullException("rainsAdInfo");

            _rainAdInfoRepository.Update(rainsAdInfo);

            //cache
            _cacheManager.RemoveByPattern(RAINSAD_PATTERN_KEY);
        }

        public RainsAdsInfo GetAdInfoById(int rainsAdInfoId)
        {
            if (rainsAdInfoId == 0)
                return null;

            string key = string.Format(RAINSAD_INFO_BY_ID_KEY, rainsAdInfoId);
            return _cacheManager.Get(key, () => { return _rainAdInfoRepository.GetById(rainsAdInfoId); });
        }

        public RainsAdsInfo GetAdInfoByWG(string widgetZone)
        {
            if (string.IsNullOrEmpty(widgetZone))
                return null;

            string key = string.Format(RAINSAD_INFO_BY_WIDGETZONE_KEY, widgetZone);
            return _cacheManager.Get(key, () =>
            {
                var query = from gp in _rainAdInfoRepository.Table
                            where gp.WidgetZone.Equals(widgetZone)
                            orderby gp.UpdatedOn descending
                            select gp;

                return query.FirstOrDefault();
            });
        }

        #endregion
    }
}
