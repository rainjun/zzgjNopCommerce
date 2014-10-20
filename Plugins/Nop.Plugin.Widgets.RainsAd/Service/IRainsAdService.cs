using System.Collections.Generic;
using Nop.Plugin.Widgets.RainsAd.Domain;
using Nop.Core;

namespace Nop.Plugin.Widgets.RainsAd.Services
{
    public partial interface IRainsAdService
    {
        IPagedList<RainsAds> GetAllAds(int widgetZoneID = 0, string widgetZoneName = null, int pageIndex = 0, int pageSize = int.MaxValue);

        void DeleteRainsAd(RainsAds rainsAd);

        void InsertRainsAd(RainsAds rainsAd);

        void UpdateRainsAd(RainsAds rainsAd);

        RainsAds GetAdById(int rainsAdId);

        IList<RainsAdItems> GetAdsItemsByAId(int aId);

        RainsAdItems GetAdItemById(int rainsAdItemId);

        void DeleteRainsAdItem(RainsAdItems rainsAdItems);

        void InsertRainsAdItem(RainsAdItems rainsAdItems);

        void UpdateRainsAdItem(RainsAdItems rainsAdItems);

        IList<RainsWidgetZones> GetAllWidgetZons(int pageIndex = 0, int pageSize = int.MaxValue);

        void DeleteRainsWidgetZone(RainsWidgetZones rainsWidgetZones);

        void InsertRainsWidgetZone(RainsWidgetZones rainsWidgetZones);

        void UpdateRainsWidgetZone(RainsWidgetZones rainsWidgetZones);

        IList<string> GetWidgetZons(int pageIndex = 0, int pageSize = int.MaxValue);

        IPagedList<RainsAdsInfo> GetAllRainsAdsInfo(int pageIndex = 0, int pageSize = int.MaxValue);

        void DeleteRainsAdInfo(RainsAdsInfo rainsAdInfo);

        void InsertRainsAdInfo(RainsAdsInfo rainsAdInfo);

        void UpdateRainsAdInfo(RainsAdsInfo rainsAdInfo);

        RainsAdsInfo GetAdInfoById(int rainsAdInfoId);
    }
}
