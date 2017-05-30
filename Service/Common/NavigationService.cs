using Common;
using Data;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.Owin;

namespace Service
{
    /// <summary>
    /// 导航业务逻辑
    /// </summary>
    public class NavigationService : INavigationService
    {
        private readonly IRepository<Navigation> _navigationRepository;
        private readonly IRepository<UserPermission> _userPermissionRepository;
        private readonly UserService _userService;

        public NavigationService(IRepository<Navigation> navigationRepository,
            IRepository<UserPermission> userPermissionRepository,
            IOwinContext context)
        {
            this._navigationRepository = navigationRepository;
            this._userPermissionRepository = userPermissionRepository;
            this._userService = context.GetUserManager<UserService>();
        }

        public virtual IQueryable<Navigation> Navigations
        {
            get
            {
                return _navigationRepository.Table;
            }
        }

        /// <summary>
        /// 根据Id获取导航
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Navigation FindById(int id)
        {
            return _navigationRepository.FirstOrDefault(id);
        }

        /// <summary>
        /// 创建导航
        /// </summary>
        /// <param name="navigation"></param>
        public void Create(Navigation navigation)
        {
            _navigationRepository.Insert(navigation);
        }

        /// <summary>
        /// 更新导航
        /// </summary>
        /// <param name="navigation"></param>
        public void Update(Navigation navigation)
        {
            _navigationRepository.Update(navigation);
        }

        /// <summary>
        /// 删除导航
        /// </summary>
        /// <param name="navigation"></param>
        public void Delete(Navigation navigation)
        {
            _navigationRepository.Delete(navigation);
        }

        /// <summary>
        /// 获取校区下的根导航
        /// </summary>
        /// <returns></returns>
        public IList<Navigation> GetRootNavigations()
        {
            var navigations = from n in _navigationRepository.Table
                              where n.ParentId == null && n.IsEnabled
                              orderby n.DisplayOrder ascending
                              select n;
            return navigations.ToList();
        }

        /// <summary>
        /// 清空学校导航
        /// </summary>
        public void ClearNavigations()
        {
            var deleteList = _navigationRepository.Table.ToList();
            _navigationRepository.Delete(deleteList);
        }

        /// <summary>
        /// 设置学校菜单权限
        /// </summary>
        /// <param name="navigationIds"></param>
        public void SetSchoolNavigations(IEnumerable<int> navigationIds)
        {
            if (navigationIds == null)
                navigationIds = new List<int>();

            var navigations = _navigationRepository.Table.Where(n => navigationIds.Contains(n.Id)).ToList();
            var permissionService = DIContainer.Resolve<IPermissionService>();
            List<int> permissionIds = new List<int>();
            ClearNavigations();

            foreach (var nav in navigations)
            {
                if (nav.ParentId.HasValue)
                    continue;

                Navigation navigation = new Navigation
                {
                    Name = nav.Name,
                    RouteName = nav.RouteName,
                    Url = nav.Url,
                    PermissionName = nav.PermissionName,
                    IconName = nav.IconName,
                    IsEnabled = nav.IsEnabled,
                    DisplayOrder = nav.DisplayOrder
                };

                if (nav.ParentId == null)
                {
                    var children = navigations.Where(n => n.ParentId != null && n.ParentId == nav.Id);
                    if (children.Count() > 0)
                    {
                        navigation.Children = new List<Navigation>();
                        foreach (var child in children)
                        {
                            Navigation childNav = new Navigation
                            {
                                Name = child.Name,
                                RouteName = child.RouteName,
                                Url = child.Url,
                                PermissionName = child.PermissionName,
                                IconName = child.IconName,
                                IsEnabled = child.IsEnabled,
                                DisplayOrder = child.DisplayOrder
                            };
                            navigation.Children.Add(childNav);

                            if (!string.IsNullOrEmpty(childNav.PermissionName))
                            {
                                var permission = permissionService.FindByName(childNav.Name);
                                if (permission != null)
                                    permissionIds.Add(permission.Id);
                            }
                        }
                    }

                }

                _navigationRepository.Insert(navigation);
                if (!string.IsNullOrEmpty(nav.PermissionName))
                {
                    var permission = permissionService.FindByName(nav.Name);
                    if (permission != null)
                        permissionIds.Add(permission.Id);
                }
            }
        }

        /// <summary>
        /// 设置用户菜单权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="navigationIds"></param>
        public void SetUserNavigations(int userId, IEnumerable<int> navigationIds)
        {
            if (navigationIds == null)
                return;

            var navigations = _navigationRepository.Table.Where(n => navigationIds.Contains(n.Id)).ToList();
            var permissionService = DIContainer.Resolve<IPermissionService>();
            List<int> permissionIds = new List<int>();

            foreach (var nav in navigations)
            {
                if (!string.IsNullOrEmpty(nav.PermissionName))
                {
                    var permission = permissionService.FindByName(nav.Name);
                    if (permission != null)
                        permissionIds.Add(permission.Id);
                }
            }

            permissionService.ClearUserPermissions(userId);
            permissionService.AddPermissionsToUser(userId, permissionIds);
        }

        /// <summary>
        /// 判断菜单名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsExists(string name)
        {
            if (_navigationRepository.FirstOrDefault(n => n.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) == null)
                return false;
            return true;
        }
    }
}
