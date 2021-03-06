﻿using AutoMapper;
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
            return Mapper.DynamicMap<RainsAdsModel, RainsAds>(model);
        }

        public static RainsAds ToEntity(this RainsAdsModel model, RainsAds destination)
        {
            return Mapper.Map(model, destination);
        }
        #endregion
    }
}
