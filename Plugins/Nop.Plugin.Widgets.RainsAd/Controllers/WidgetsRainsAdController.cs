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
    /// <summary>
    /// 缺少了缓存
    /// </summary>
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

        #region tools

        protected RainsAdsInfo ToEntityInfo(RainsAdsInfoModel model)
        {
            if (model == null)
                return null;

            RainsAdsInfo entity = new RainsAdsInfo();
            entity.Id = model.Id;
            entity.Name = model.Name;
            entity.PictureId = model.PictureId;
            entity.PictureCount = model.PictureCount;
            entity.Text = model.Text;
            entity.WidgetZone = model.WidgetZone;
            entity.CreatedOn = model.CreatedOn;
            entity.UpdatedOn = model.UpdatedOn;

            entity.Link1 = model.Link1;
            entity.Tilte1 = model.Tilte1;
            entity.PictureId1 = model.PictureId1;

            entity.Link2 = model.Link2;
            entity.Tilte2 = model.Tilte2;
            entity.PictureId2 = model.PictureId2;

            entity.Link3 = model.Link3;
            entity.Tilte3 = model.Tilte3;
            entity.PictureId3 = model.PictureId3;

            entity.Link4 = model.Link4;
            entity.Tilte4 = model.Tilte4;
            entity.PictureId4 = model.PictureId4;

            entity.Link5 = model.Link5;
            entity.Tilte5 = model.Tilte5;
            entity.PictureId5 = model.PictureId5;

            entity.Link6 = model.Link6;
            entity.Tilte6 = model.Tilte6;
            entity.PictureId6 = model.PictureId6;

            entity.Link7 = model.Link7;
            entity.Tilte7 = model.Tilte7;
            entity.PictureId7 = model.PictureId7;

            return entity;
        }

        protected RainsAdsInfoModel ToModelInfo(RainsAdsInfo entity)
        {
            if (entity == null)
                return null;

            RainsAdsInfoModel model = new RainsAdsInfoModel();
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.PictureId = entity.PictureId;
            model.PictureCount = entity.PictureCount;
            model.Text = entity.Text;
            model.WidgetZone = entity.WidgetZone;
            model.CreatedOn = entity.CreatedOn;
            model.UpdatedOn = entity.UpdatedOn;

            model.Link1 = entity.Link1;
            model.Tilte1 = entity.Tilte1;
            model.PictureId1 = entity.PictureId1;

            model.Link2 = entity.Link2;
            model.Tilte2 = entity.Tilte2;
            model.PictureId2 = entity.PictureId2;

            model.Link3 = entity.Link3;
            model.Tilte3 = entity.Tilte3;
            model.PictureId3 = entity.PictureId3;

            model.Link4 = entity.Link4;
            model.Tilte4 = entity.Tilte4;
            model.PictureId4 = entity.PictureId4;

            model.Link5 = entity.Link5;
            model.Tilte5 = entity.Tilte5;
            model.PictureId5 = entity.PictureId5;

            model.Link6 = entity.Link6;
            model.Tilte6 = entity.Tilte6;
            model.PictureId6 = entity.PictureId6;

            model.Link7 = entity.Link7;
            model.Tilte7 = entity.Tilte7;
            model.PictureId7 = entity.PictureId7;

            return model;
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

        #endregion

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
            var rainsAdInfo = _rainsAdService.GetAdInfoByWG(widgetZone);

            var model = rainsAdInfo.ToModel();

            var picUrl = GetPictureUrl(model.PictureId1);
            //名称或者图片其中一个必须要有值
            if (!string.IsNullOrEmpty(picUrl) || !string.IsNullOrEmpty(model.Tilte1))
            {
                model.Items.Add(new RainsAdItemsModel { Name = model.Tilte1, Link = model.Link1, PictureId = model.PictureId1, PictureUrl = picUrl });
            }

            picUrl = GetPictureUrl(model.PictureId2);
            //名称或者图片其中一个必须要有值
            if (!string.IsNullOrEmpty(picUrl) || !string.IsNullOrEmpty(model.Tilte2))
            {
                model.Items.Add(new RainsAdItemsModel { Name = model.Tilte2, Link = model.Link2, PictureId = model.PictureId2, PictureUrl = picUrl });
            }

            picUrl = GetPictureUrl(model.PictureId3);
            //名称或者图片其中一个必须要有值
            if (!string.IsNullOrEmpty(picUrl) || !string.IsNullOrEmpty(model.Tilte3))
            {
                model.Items.Add(new RainsAdItemsModel { Name = model.Tilte3, Link = model.Link3, PictureId = model.PictureId3, PictureUrl = picUrl });
            }

            picUrl = GetPictureUrl(model.PictureId4);
            //名称或者图片其中一个必须要有值
            if (!string.IsNullOrEmpty(picUrl) || !string.IsNullOrEmpty(model.Tilte4))
            {
                model.Items.Add(new RainsAdItemsModel { Name = model.Tilte4, Link = model.Link4, PictureId = model.PictureId4, PictureUrl = picUrl });
            }

            picUrl = GetPictureUrl(model.PictureId5);
            //名称或者图片其中一个必须要有值
            if (!string.IsNullOrEmpty(picUrl) || !string.IsNullOrEmpty(model.Tilte5))
            {
                model.Items.Add(new RainsAdItemsModel { Name = model.Tilte5, Link = model.Link5, PictureId = model.PictureId5, PictureUrl = picUrl });
            }

            picUrl = GetPictureUrl(model.PictureId6);
            //名称或者图片其中一个必须要有值
            if (!string.IsNullOrEmpty(picUrl) || !string.IsNullOrEmpty(model.Tilte6))
            {
                model.Items.Add(new RainsAdItemsModel { Name = model.Tilte6, Link = model.Link6, PictureId = model.PictureId6, PictureUrl = picUrl });
            }

            picUrl = GetPictureUrl(model.PictureId7);
            //名称或者图片其中一个必须要有值
            if (!string.IsNullOrEmpty(picUrl) || !string.IsNullOrEmpty(model.Tilte7))
            {
                model.Items.Add(new RainsAdItemsModel { Name = model.Tilte7, Link = model.Link7, PictureId = model.PictureId7, PictureUrl = picUrl });
            }

            if (model.Items.Count <= 0)
                //no pictures uploaded
                return Content("");

            //地址
            //本项目的views或者
            //使用的皮肤下Nop.Web\Themes\皮肤名称\Views\WidgetsRainsAd\Nop.Plugin.Widgets.RainsAd.Views.WidgetsRainsAd.PublicInfo.使用的名称.cshtml
            return View("Nop.Plugin.Widgets.RainsAd.Views.WidgetsRainsAd.PublicInfo." + widgetZone, model);
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

        public ActionResult RainsAdUpdate(int Id)
        {
            if (Id <= 0)
                return RedirectToAction("Configure");
            var ad = _rainsAdService.GetAdById(Id);
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


        public ActionResult RainsAdInfoUpdate(int Id)
        {
            if (Id <= 0)
                return RedirectToAction("Configure");
            var ad = _rainsAdService.GetAdInfoById(Id);
            if (null == ad)
            {
                return RedirectToAction("Configure");
            }
            RainsAdsInfoModel model = ad.ToModel();
            //RainsAdsInfoModel model = ToModelInfo(ad);

            return View("Nop.Plugin.Widgets.RainsAd.Views.WidgetsRainsAd.RainsAdInfoUpdate", model);
        }

        [HttpPost]
        public ActionResult RainsAdInfoUpdate(string btnId, string formId, RainsAdsInfoModel model)
        {
            var ad = _rainsAdService.GetAdInfoById(model.Id);
            if (ad == null)
                //No category found with the specified id
                return RedirectToAction("Configure");
            model.CreatedOn = ad.CreatedOn;

            ad = model.ToEntity(ad);
            // ad = ToEntityInfo(model);
            //update需要对当前entity做修改
            ad.UpdatedOn = DateTime.UtcNow;
            _rainsAdService.UpdateRainsAdInfo(ad);


            ViewBag.RefreshPage = true;
            ViewBag.btnId = btnId;
            ViewBag.formId = formId;

            return View("Nop.Plugin.Widgets.RainsAd.Views.WidgetsRainsAd.RainsAdInfoUpdate", model);
        }

        public ActionResult CreateAdInfo()
        {
            RainsAdsInfoModel model = new RainsAdsInfoModel();

            return View("Nop.Plugin.Widgets.RainsAd.Views.WidgetsRainsAd.CreateAdInfo", model);
        }
        [HttpPost]
        public ActionResult CreateAdInfo(string btnId, string formId, RainsAdsInfoModel model, bool continueEditing = false)
        {
            ViewBag.RefreshPage = true;
            ViewBag.btnId = btnId;
            ViewBag.formId = formId;

            var rainsAd = model.ToEntity();//假如这个不行
            //var rainsAd = ToEntityInfo(model);
            rainsAd.CreatedOn = DateTime.UtcNow;
            rainsAd.UpdatedOn = DateTime.UtcNow;

            _rainsAdService.InsertRainsAdInfo(rainsAd);

            //return RedirectToAction("RainsAdInfoUpdate", new { id = rainsAd.Id });

            return View("Nop.Plugin.Widgets.RainsAd.Views.WidgetsRainsAd.CreateAdInfo", model);
        }
        #endregion
    }
}