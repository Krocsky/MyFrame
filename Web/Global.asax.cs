using Autofac;
using Autofac.Integration.Mvc;
using Common.Caching;
using Common;
using Data;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Common.Utilities;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using Service;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //依赖注入
            RegisterDependencies();
            //初始化
            Initialize();
            //注册路由
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //注册bundle
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //执行排课自运行队列
            //QueueTaskManageService.Instance.Start();
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
            //注册Owin
            builder.Register(c => HttpContext.Current.GetOwinContext()).As<IOwinContext>();
            //Asp.Net Identity Store
            builder.RegisterType<UserStore>().AsSelf().As<IUserStore<User, int>>();
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
            //注册控制器
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //注册自运行任务
            builder.RegisterType<QuartzTaskScheduler>().As<ITaskScheduler>().SingleInstance();

            var container = builder.Build();
            DIContainer.SetContainer(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
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

            //开启自运行任务
            ITaskScheduler taskScheduler = DIContainer.Resolve<ITaskScheduler>();
            taskScheduler.Start();
        }
    }
}
