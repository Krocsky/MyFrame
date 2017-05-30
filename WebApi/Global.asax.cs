using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Common.Utilities;
using Data;
using Common.Caching;
using Common;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using System.Reflection;
using System.Web.Http;
using Autofac.Integration.WebApi;
using Autofac.Integration.Mvc;
using WebApi.Models;
using WebApi.Providers;

namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //依赖注入
            RegisterDependencies();

            Initialize();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// 依赖注入
        /// </summary>
        private void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            var finder = new WebAppTypeFinder();
            //获取所有程序集
            var assemblys = finder.GetAssemblies().ToArray();
            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            //注册Owin
            builder.Register(c => HttpContext.Current.GetOwinContext()).As<IOwinContext>();
            //Asp.Net Identity Store
            builder.RegisterType<UserStore>().AsSelf().As<IUserStore<Common.User, int>>();
            builder.RegisterType<RoleStore>();
            //批量注册Service
            builder.RegisterAssemblyTypes(assemblys).Where(t => t.Name.EndsWith("Service"))
                .AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
            //批量注册Repository
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfRepository<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();
            //注册事件
            builder.RegisterTypes(finder.FindClassesOfType<IEvent>().ToArray()).As<IEvent>().SingleInstance();
            //注册DBContext
            builder.Register<EFDbContext>(c => new EFDbContext("MySqlServer")).InstancePerLifetimeScope();
            //注册内存缓存
            builder.RegisterType<MemoryCacheService>().As<ICacheService>().SingleInstance();
            //注册类型查找器
            builder.RegisterInstance(finder).As<ITypeFinder>().SingleInstance();
            //注册Log4Net
            builder.RegisterType<Log4Net>().As<ILogger>().SingleInstance();

            // Register your Web API controllers.
            var controllers = Assembly.GetExecutingAssembly();
            builder.RegisterApiControllers(controllers);
            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            var container = builder.Build();
            DIContainer.SetContainer(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            // Set the dependency resolver to be Autofac.
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        /// <summary>
        /// 应用初始化
        /// </summary>
        private void Initialize()
        {
            //注册事件处理程序
            IEnumerable<IEvent> events = DIContainer.Resolve<IEnumerable<IEvent>>();
            foreach (var e in events)
            {
                e.HandleEvent();
            }
        }
    }
}
