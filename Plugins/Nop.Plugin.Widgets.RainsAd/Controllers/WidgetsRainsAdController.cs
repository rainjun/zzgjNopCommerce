using AutoMapper;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.RainsAd.Domain;
using Nop.Plugin.Widgets.RainsAd.Infrastructure.Cache;
using Nop.Plugin.Widgets.RainsAd.Models;
using Nop.Plugin.Widgets.RainsAd.Services;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Nop.Plugin.Widgets.RainsAd.Controllers
{
    public class WidgetsRainsAdController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;
        private readonly IRainsAdService _rainsAdService;

        public WidgetsRainsAdController(IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService,
            IPictureService pictureService,
            ISettingService settingService,
            ICacheManager cacheManager,
            IRainsAdService rainsAdService)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._cacheManager = cacheManager;
            this._rainsAdService = rainsAdService;
        }

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

        protected void BindPictureCount(ConfigurationModel model)
        {
            int pcount = 7;
            for (int i = 1; i <= pcount; i++)
            {
                model.AvailablePictureCount.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
        }

        protected void BindWidgetZones(ConfigurationModel model)
        {
            var allWidgetZones = _rainsAdService.GetAllWidgetZons();
            List<SelectListItem> widgetZonesSel = new List<SelectListItem>();
            foreach (var widgetZone in allWidgetZones)
            {
                widgetZonesSel.Add(new SelectListItem { Text = widgetZone.Name, Value = widgetZone.Id.ToString() });
            }
            widgetZonesSel.Insert(0, new SelectListItem
            {
                Text = "[None]",
                Value = "0"
            });
            model.AvailableWidgetZones = widgetZonesSel;
        }

        protected void PrepareRainsAdModel(RainsAdsModel model)
        {
            model.AvailablePictureCount = new List<SelectListItem>();
            for (int i = 1; i <= 7; i++)
            {
                model.AvailablePictureCount.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            var allWidgetZones = _rainsAdService.GetAllWidgetZons();
            List<SelectListItem> widgetZonesSel = new List<SelectListItem>();
            foreach (var widgetZone in allWidgetZones)
            {
                widgetZonesSel.Add(new SelectListItem { Text = widgetZone.Name, Value = widgetZone.Id.ToString() });
            }
            widgetZonesSel.Insert(0, new SelectListItem
            {
                Text = "[None]",
                Value = "0"
            });
            model.AvailableWidgetZones = widgetZonesSel;

        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var RainsAdSettings = _settingService.LoadSetting<RainsAdSettings>(storeScope);
            var model = new ConfigurationModel();

            BindPictureCount(model);
            BindWidgetZones(model);

            return View("Nop.Plugin.Widgets.RainsAd.Views.WidgetsRainsAd.Configure", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var RainsAdSettings = _settingService.LoadSetting<RainsAdSettings>(storeScope);


            return Configure();
        }



        [ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone)
        {
            var RainsAdSettings = _settingService.LoadSetting<RainsAdSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoModel();
            model.Picture1Url = GetPictureUrl(RainsAdSettings.Picture1Id);
            model.Text1 = RainsAdSettings.Text1;
            model.Link1 = RainsAdSettings.Link1;

            model.Picture2Url = GetPictureUrl(RainsAdSettings.Picture2Id);
            model.Text2 = RainsAdSettings.Text2;
            model.Link2 = RainsAdSettings.Link2;

            model.Picture3Url = GetPictureUrl(RainsAdSettings.Picture3Id);
            model.Text3 = RainsAdSettings.Text3;
            model.Link3 = RainsAdSettings.Link3;

            model.Picture4Url = GetPictureUrl(RainsAdSettings.Picture4Id);
            model.Text4 = RainsAdSettings.Text4;
            model.Link4 = RainsAdSettings.Link4;

            model.Picture5Url = GetPictureUrl(RainsAdSettings.Picture5Id);
            model.Text5 = RainsAdSettings.Text5;
            model.Link5 = RainsAdSettings.Link5;

            if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
                string.IsNullOrEmpty(model.Picture3Url) && string.IsNullOrEmpty(model.Picture4Url) &&
                string.IsNullOrEmpty(model.Picture5Url))
                //no pictures uploaded
                return Content("");


            return View("Nop.Plugin.Widgets.RainsAd.Views.WidgetsRainsAd.PublicInfo", model);
        }

        #region  插件内部的action

        [HttpPost]
        public ActionResult RainsAdsList(DataSourceRequest command, RainsAdsListModel model)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
            //    return Content("Access denied");

            var ads = _rainsAdService.GetAllAds(
                widgetZoneID: model.WidgetZoneID,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = ads.Select(x =>
                {
                    var entityModel = x.ToModel();
                    return entityModel;
                }),
                Total = ads.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult RainsAdsInfoList(DataSourceRequest command, RainsAdsListModel model)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
            //    return Content("Access denied");

            var ads = _rainsAdService.GetAllRainsAdsInfo(
                pageIndex: command.Page - 1,
                pageSize: command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = ads.Select(x =>
                {
                    var entityModel = x.ToModel();
                    return entityModel;
                }),
                Total = ads.TotalCount
            };

            return Json(gridModel);
        }

        public ActionResult RainsAdUpdate(int adId)
        {
            if (adId <= 0)
                return RedirectToAction("Configure");
            var ad = _rainsAdService.GetAdById(adId);
            if (null == ad)
            {
                return RedirectToAction("Configure");
            }
            RainsAdsModel model = ad.ToModel();

            return View("Nop.Plugin.Widgets.RainsAd.Views.WidgetsRainsAd.RainsAdUpdate", model);
        }

        [HttpPost]
        public ActionResult RainsAdUpdate(RainsAdsModel model)
        {
            var ad = _rainsAdService.GetAdById(model.Id);
            if (ad == null)
                //No category found with the specified id
                return RedirectToAction("Configure");

            ad = model.ToEntity(ad);
            ad.UpdateOn = DateTime.UtcNow;
            _rainsAdService.UpdateRainsAd(ad);

            return View("Nop.Plugin.Widgets.RainsAd.Views.WidgetsRainsAd.RainsAdUpdate", model);
        }

        public ActionResult CreateAd()
        {
            RainsAdsModel model = new RainsAdsModel();

            PrepareRainsAdModel(model);

            return View("Nop.Plugin.Widgets.RainsAd.Views.WidgetsRainsAd.CreateAd", model);
        }
        [HttpPost]
        public ActionResult CreateAd(RainsAdsModel model, bool continueEditing = false)
        {
            if (ModelState.IsValid)
            {
                var rainsAd = model.ToEntity();
                rainsAd.CreateOn = DateTime.UtcNow;
                rainsAd.UpdateOn = DateTime.UtcNow;

                return continueEditing ? RedirectToAction("RainsAdUpdate", new { id = rainsAd.Id }) : RedirectToAction("Configure");
            }

            PrepareRainsAdModel(model);

            return View("Nop.Plugin.Widgets.RainsAd.Views.WidgetsRainsAd.CreateAd", model);
        }
        #endregion
    }
}