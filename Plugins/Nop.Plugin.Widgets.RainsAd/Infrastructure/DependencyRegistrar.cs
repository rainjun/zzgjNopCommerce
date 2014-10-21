using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Widgets.RainsAd.Controllers;
using Nop.Plugin.Widgets.RainsAd.Data;
using Nop.Plugin.Widgets.RainsAd.Domain;
using Nop.Plugin.Widgets.RainsAd.Services;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.RainsAd.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<RainsAdService>().As<IRainsAdService>().InstancePerHttpRequest();
            //we cache presentation models between requests
            builder.RegisterType<WidgetsRainsAdController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"));

            //data context
            this.RegisterPluginDataContext<RainsAdObjectContext>(builder, "nop_object_context_rains_ad");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<RainsAdsInfo>>()
                .As<IRepository<RainsAdsInfo>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_rains_ad"))
                .InstancePerLifetimeScope();

            //override required repository with our custom context
            builder.RegisterType<EfRepository<RainsAds>>()
                .As<IRepository<RainsAds>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_rains_ad"))
                .InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<RainsAdItems>>()
                .As<IRepository<RainsAdItems>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_rains_ad"))
                .InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<RainsWidgetZones>>()
                .As<IRepository<RainsWidgetZones>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_rains_ad"))
                .InstancePerLifetimeScope();
        }

        public int Order
        {
            get { return 1; }
        }
    }
}
