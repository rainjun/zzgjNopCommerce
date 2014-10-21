using AutoMapper;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.RainsAd.Domain;
using Nop.Plugin.Widgets.RainsAd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.RainsAd.Data
{
    public class AutoMapperStartupTask : IStartupTask
    {
        public void Execute()
        {
            //TODO remove 'CreatedOnUtc' ignore mappings because now presentation layer models have 'CreatedOn' property and core entities have 'CreatedOnUtc' property (distinct names)

            Mapper.CreateMap<RainsAdsInfo, RainsAdsInfoModel>();
            Mapper.CreateMap<RainsAds, RainsAdsModel>();
        }

        public int Order
        {
            //ensure that this task is run first 
            get { return 0; }
        }
    }
}
