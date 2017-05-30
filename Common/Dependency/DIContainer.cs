using System;
using System.Web;
using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;

namespace Common
{
    /// <summary>
    /// 依赖注入容器
    /// </summary>
    public static class DIContainer
    {
        private static IContainer _container;

        /// <summary>
        /// 设置容器
        /// </summary>
        /// <param name="container"></param>
        public static void SetContainer(IContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            if (scope == null)
            {
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<T>();
            }
            return scope.ResolveKeyed<T>(key);
        }

        /// <summary>
        /// 根据类型获取实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static object Resolve(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            return scope.Resolve(type);
        }

        /// <summary>
        /// 生命周期
        /// </summary>
        /// <returns></returns>
        private static ILifetimeScope Scope()
        {
            try
            {
                if (HttpContext.Current != null)
                    return AutofacDependencyResolver.Current.RequestLifetimeScope;

                return _container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
            catch (Exception ex)
            {
                return _container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
        }
    }
}
