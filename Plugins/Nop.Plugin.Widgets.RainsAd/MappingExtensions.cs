using AutoMapper;
using Nop.Plugin.Widgets.RainsAd.Domain;
using Nop.Plugin.Widgets.RainsAd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.RainsAd
{
    public static class MappingExtensions
    {
        #region RainsAds

        public static RainsAdsModel ToModel(this RainsAds entity)
        {
            return Mapper.Map<RainsAds, RainsAdsModel>(entity);
        }

        public static RainsAds ToEntity(this RainsAdsModel model)
        {
            return Mapper.Map<RainsAdsModel, RainsAds>(model);
        }

        public static RainsAds ToEntity(this RainsAdsModel model, RainsAds destination)
        {
            return Mapper.Map(model, destination);
        }
        #endregion

        #region RainsAdsInfo

        public static RainsAdsInfoModel ToModel(this RainsAdsInfo entity)
        {
            return Mapper.Map<RainsAdsInfo, RainsAdsInfoModel>(entity);
        }

        public static RainsAdsInfo ToEntity(this RainsAdsInfoModel model)
        {
            return Mapper.DynamicMap<RainsAdsInfoModel, RainsAdsInfo>(model);
        }

        public static RainsAdsInfo ToEntity(this RainsAdsInfoModel model, RainsAdsInfo destination)
        {
            Mapper.DynamicMap(model, destination);
            return destination;
        }
        #endregion
    }
}
