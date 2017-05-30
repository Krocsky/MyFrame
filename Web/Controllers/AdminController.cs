using Common.Caching;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Web.ViewModels;
using Microsoft.Owin;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;
using Data;
using Common.Extensions;
using Common.Utilities;

namespace Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        #region Ctor

        private readonly ICacheService _cacheManager;
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly IPermissionService _permissionService;
        private readonly INavigationService _navigationService;
        private readonly EFDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IAdminService _adminService;
        public readonly IChinaAreaService _chinaAreaService;
        private readonly IQuartzTaskService _quartzTaskService;

        public AdminController(ICacheService cacheManager, IOwinContext context, IPermissionService permissionService, INavigationService navigationService, EFDbContext dbContext, ILogger logger, IAdminService adminService, IChinaAreaService chinaAreaService, IQuartzTaskService quartzTaskService)
        {
            this._cacheManager = cacheManager;
            this._userService = context.GetUserManager<UserService>();
            this._roleService = context.Get<RoleService>();
            this._permissionService = permissionService;
            this._navigationService = navigationService;
            this._dbContext = dbContext;
            this._logger = logger;
            this._adminService = adminService;
            this._chinaAreaService = chinaAreaService;
            this._quartzTaskService = quartzTaskService;
        }

        #endregion

        #region 权限

        /// <summary>
        /// 角色管理
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult ManageRoles(int pageIndex = 1, int pageSize = 20)
        {
            var list = new PagedList<Role>(_roleService.Roles, pageIndex, pageSize, n => n.DisplayOrder);
            return View(list);
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditRole(int? id = null)
        {
            Role role;
            if (id == null)
                role = new Role();
            else
                role = _roleService.FindById(id.Value);

            if (role == null)
                return HttpNotFound();

            ViewData["roleType"] = RoleType.System.ToSelectListItemNoEmpty();
            return View(role.MapTo<RoleEditModel>());
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [PermissionCheck(Permission = "角色管理")]
        [HttpPost]
        public async Task<JsonResult> EditRole(RoleEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new MessageData(false, "操作失败"));
            }

            Role role = null;
            IdentityResult result = null;

            if (model.Id > 0)
            {
                role = _roleService.FindById(model.Id);
                if (role == null)
                    return Json(new MessageData(false, "角色不存在"));

                role.Type = model.Type;
                role.DisplayOrder = model.DisplayOrder;
                role.Description = model.Description;

                result = await _roleService.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return Json(new MessageData(true, "保存成功"));
                }
            }
            else
            {
                if (await _roleService.RoleExistsAsync(model.Name))
                {
                    return Json(new MessageData(false, "角色已存在"));
                }

                role = new Role(model.Name);
                role.Type = model.Type;
                role.DisplayOrder = model.DisplayOrder;
                role.Description = model.Description;

                result = await _roleService.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Json(new MessageData(true, "添加成功"));
                }
            }

            return Json(new MessageData(false, result.Errors.First()));
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [PermissionCheck(Permission = "角色管理")]
        [HttpPost]
        public async Task<JsonResult> DeleteRole(int id)
        {
            var role = await _roleService.FindByIdAsync(id);
            if (role == null)
                return Json(new MessageData(false, "角色不存在"));

            if (role.IsBuiltIn)
                return Json(new MessageData(false, "删除失败，系统内置角色不可删除"));

            var result = await _roleService.DeleteAsync(role);
            if (result.Succeeded)
            {
                _permissionService.ClearRolePermissions(role.Id, null);
                return Json(new MessageData(true, "删除成功"));
            }

            return Json(new MessageData(false, result.Errors.First()));
        }
        /// <summary>
        /// 菜单权限分配
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [PermissionCheck(Permission = "菜单权限分配")]
        public ActionResult ManageDistributionPermission(int id)
        {
            //获取菜单的所有权限
            List<Permission> PermissionList = _permissionService.Permissions.Where(n => n.RolePermissonEnumType == RolePermissonEnum.Navigation).ToList();
            //获取菜单的所有父节点
            List<Permission> parentPermissionList = PermissionList.Where(n => n.ParentId == null).ToList();
            //获取当前role所拥有的菜单权限
            int[] permissionIdList = _permissionService.GetRolePermissions(id).Where(n => n.RolePermissonEnumType == RolePermissonEnum.Navigation).Where(n => n.IsButton == false).Select(n => n.Id).ToList().ToArray();
            string TabsList = string.Join(",", permissionIdList);
            List<DistributionTabs> distributionTabsList = new List<DistributionTabs>();
            foreach (var item in parentPermissionList)
            {
                DistributionTabs distributionTabs = new DistributionTabs();
                distributionTabs.folder = true;
                distributionTabs.title = item.Name;
                distributionTabs.key = item.Id;
                List<Permission> childrenPermissionList = PermissionList.Where(n => n.ParentId == item.Id).ToList();
                if (childrenPermissionList.Count != 0)
                {
                    foreach (var children in childrenPermissionList)
                    {
                        DistributionTabs childrenDistributionTabs = new DistributionTabs();
                        childrenDistributionTabs.folder = true;
                        childrenDistributionTabs.title = children.Name;
                        childrenDistributionTabs.key = children.Id;
                        if (permissionIdList.Contains(childrenDistributionTabs.key))
                        {
                            childrenDistributionTabs.selected = true;
                        }
                        distributionTabs.children.Add(childrenDistributionTabs);
                    }
                }
                distributionTabsList.Add(distributionTabs);
            }
            var jsonNew = JsonConvert.SerializeObject(distributionTabsList);
            ViewData["RoleId"] = id;
            ViewData["jsonNew"] = jsonNew;
            ViewData["TabsList"] = TabsList;
            return View();
        }
        /// <summary>
        /// 页签权限分配
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [PermissionCheck(Permission = "页签权限分配")]
        public ActionResult ManageDistributionTabs(int id)
        {
            //获取标签页的所有权限
            List<Permission> PermissionList = _permissionService.Permissions.Where(n => n.RolePermissonEnumType == RolePermissonEnum.Tabs).ToList();
            //获取标签页的所有父节点
            List<Permission> parentPermissionList = PermissionList.Where(n => n.ParentId == null).ToList();
            //获取当前role所拥有的标签页权限
            int[] permissionIdList = _permissionService.GetRolePermissions(id).Where(n => n.RolePermissonEnumType == RolePermissonEnum.Tabs).Where(n => n.IsButton == false).Select(n => n.Id).ToList().ToArray();
            string TabsList = string.Join(",", permissionIdList);
            List<DistributionTabs> distributionTabsList = new List<DistributionTabs>();
            foreach (var item in parentPermissionList)
            {
                DistributionTabs distributionTabs = new DistributionTabs();
                distributionTabs.folder = true;
                distributionTabs.title = item.Name;
                distributionTabs.key = item.Id;
                List<Permission> childrenPermissionList = PermissionList.Where(n => n.ParentId == item.Id).ToList();
                if (childrenPermissionList.Count != 0)
                {
                    foreach (var children in childrenPermissionList)
                    {
                        DistributionTabs childrenDistributionTabs = new DistributionTabs();
                        childrenDistributionTabs.folder = true;
                        childrenDistributionTabs.title = children.Name;
                        childrenDistributionTabs.key = children.Id;
                        if (permissionIdList.Contains(childrenDistributionTabs.key))
                        {
                            childrenDistributionTabs.selected = true;
                        }
                        distributionTabs.children.Add(childrenDistributionTabs);
                    }
                }
                distributionTabsList.Add(distributionTabs);
            }
            var jsonNew = JsonConvert.SerializeObject(distributionTabsList);
            ViewData["RoleId"] = id;
            ViewData["jsonNew"] = jsonNew;
            ViewData["TabsList"] = TabsList;
            return View();
        }
        /// <summary>
        /// 页签权限分配
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DistributionTabs(List<int> like)
        {
            string tabsList = Request.Form["TabsList"];
            if (string.IsNullOrWhiteSpace(tabsList))
            {
                return Json(new MessageData(false, "未做任何修改,分配失败"));
            }
            int roleId = int.Parse(Request.Form["RoleId"]);
            //清空标签页权限
            _permissionService.ClearRolePermissions(roleId, RolePermissonEnum.Tabs);
            //增加标签页权限
            string[] tabsArray = tabsList.Split(',');
            //标签页及标签页button
            List<int> intButtonList = new List<int>();
            foreach (var item in tabsArray.ToList())
            {
                intButtonList.Add(int.Parse(item));
            }
            //增加标签页button权限
            if (like != null)
            {
                string[] tabsButtonArray = like.Select(n => n.ToString()).ToArray();
                foreach (var item in tabsButtonArray.ToList())
                {
                    intButtonList.Add(int.Parse(item));
                }
            }
            _permissionService.AddPermissionsToRole(roleId, intButtonList, RolePermissonEnum.Tabs);
            return Json(new MessageData(true, "分配成功"));
        }

        /// <summary>
        /// 菜单权限分配
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DistributionNavigation(List<int> like)
        {
            string tabsList = Request.Form["TabsList"];
            if (string.IsNullOrWhiteSpace(tabsList))
            {
                return Json(new MessageData(false, "未做任何修改,分配失败"));
            }
            int roleId = int.Parse(Request.Form["RoleId"]);
            _permissionService.ClearRolePermissions(roleId, RolePermissonEnum.Navigation);
            string[] tabsArray = tabsList.Split(',');
            //Navigation页及Navigation页button
            List<int> intNavigationList = new List<int>();
            foreach (var item in tabsArray.ToList())
            {
                intNavigationList.Add(int.Parse(item));
            }
            //手动增加父节点的角色权限
            //_permissionService.FindById(intNavigationList.LastOrDefault()).ParentId.Value;
            //intNavigationList.Add();
            //增加标签页button权限
            if (like != null)
            {
                string[] navigationListButtonArray = like.Select(n => n.ToString()).ToArray();
                foreach (var item in navigationListButtonArray.ToList())
                {
                    intNavigationList.Add(int.Parse(item));
                }
            }
            try
            {
                _permissionService.AddPermissionsToRole(roleId, intNavigationList, RolePermissonEnum.Navigation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new MessageData(true, "分配成功"));
        }
        /// <summary>
        /// 获取标签页buttons
        /// </summary>
        /// <param name="key">父节点Id</param>
        /// <param name="roleId">当前角色</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTabsButtons(int key, int roleId)
        {
            List<ListItem> listItemList = new List<ListItem>();
            Permission permission = _permissionService.Permissions.Where(n => n.Id == key).FirstOrDefault();
            //获取当前role所拥有的标签页button权限
            int[] permissionIdList = _permissionService.GetRolePermissions(roleId).Where(n => n.RolePermissonEnumType == permission.RolePermissonEnumType).Where(n => n.IsButton == true).Select(n => n.Id).ToList().ToArray();

            if (_permissionService.Permissions.Where(n => n.Id == key).FirstOrDefault().ParentId != null)
            {
                List<Permission> permissonList = _permissionService.Permissions.Where(n => n.ParentId == key).ToList();
                if (permissonList.Count() != 0)
                {
                    foreach (var item in permissonList)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = item.Name.Substring(item.Name.LastIndexOf("-") + 1);
                        listItem.Value = item.Id.ToString();
                        if (permissionIdList.Contains(item.Id))
                        {
                            listItem.Selected = true;
                        }
                        listItemList.Add(listItem);
                    }
                }
            }
            return Json(listItemList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [PermissionCheck(Permission = "用户管理")]
        [HttpGet]
        public async Task<ActionResult> EditUser(int id)
        {
            var currentUser = UserContext.CurrentUser;
            var user = await _userService.FindByIdAsync(id);
            if (user == null)
                return HttpNotFound();
            ViewData["UserType"] = UserType.Customer.ToSelectListItemNoEmpty();
            var model = new UserCreateModel();
            model.UserName = user.UserName;
            model.LoginAccount = user.UserName;
            model.PhoneNumber = user.PhoneNumber;
            model.Description = user.Description;
            model.ProvinceName = (user.ProvinceId.HasValue ? user.ProvinceTable.Name : "");
            model.AreaName = (user.AreaId.HasValue ? user.AreaTable.Name : "");
            model.CityName = (user.CityId.HasValue ? user.CityTable.Name : "");

            model.UserType = user.UserType;
            model.Roles = _roleService.Roles.ToList();
            return View(model);
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [PermissionCheck(Permission = "用户管理")]
        [HttpPost]
        public async Task<ActionResult> EditUser(UserCreateModel model, string[] role)
        {
            var user = await _userService.FindByIdAsync(model.Id);
            if (user == null)
                return Json(new MessageData(false, "用户不存在"));
            var province = _chinaAreaService.FindChinaAreaByCode(model.ProvinceCode);
            var city = _chinaAreaService.FindChinaAreaByCode(model.CityCode);
            var area = _chinaAreaService.FindChinaAreaByCode(model.AreaCode);
            user.TrueName = model.UserName;
            user.UserName = model.LoginAccount;
            user.PhoneNumber = model.PhoneNumber;
            user.Description = model.Description;
            user.ProvinceId = province.Id;
            user.CityId = city.Id;
            user.AreaId = area.Id;
            user.Roles.Clear();

            var result = await _userService.UpdateAsync(user);

            if (result.Succeeded)
            {
                if (role != null)
                {
                    foreach (var single in role)
                    {
                        result = await _userService.AddToRolesAsync(user.Id, single);
                    }
                }

                if (result.Succeeded)
                {
                    return Json(new MessageData(true, "保存成功"));
                }
            }

            return Json(new MessageData(false, result.Errors.First()));
        }

        /// <summary>
        /// 管理角色下的用户
        /// </summary>
        /// <returns></returns>
        [PermissionCheck(Permission = "角色管理")]
        public ActionResult ManageRoleUsers()
        {
            return View(_roleService.Roles.ToList());
        }

        /// <summary>
        /// 角色用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [PermissionCheck(Permission = "角色管理")]
        public ActionResult _RoleUsers(int id, int pageIndex = 1, int pageSize = 20)
        {
            var role = _roleService.FindById(id);
            if (role == null)
                return View();

            var list = new PagedList<User>(role.Users.Select(n => _userService.FindById(n.UserId)).Where(n => !n.IsDelete).ToList(), pageIndex, pageSize);
            ViewData["roleId"] = id;
            ViewBag.RoleName = role.Name;
            return View(list);
        }

        /// <summary>
        /// 添加用户到指定角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [PermissionCheck(Permission = "角色管理")]
        [HttpGet]
        public async Task<ActionResult> AddUserToRole(int roleId)
        {
            var role = await _roleService.FindByIdAsync(roleId);
            if (role == null)
                return HttpNotFound();

            return View(role.MapTo<RoleEditModel>());
        }

        /// <summary>
        /// 添加用户到指定角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [PermissionCheck(Permission = "角色管理")]
        [HttpPost]
        public async Task<JsonResult> AddUserToRole(int roleId, int userId)
        {
            var role = await _roleService.FindByIdAsync(roleId);
            if (role == null)
                return Json(new MessageData(false, "角色不存在"));

            User user = await _userService.FindByIdAsync(userId);
            if (user == null)
                return Json(new MessageData(false, "用户不存在"));

            if (user.HasRole(role.Name))
                return Json(new MessageData(false, "用户已有此角色"));

            var result = await _userService.AddToRoleAsync(userId, role.Name);
            if (result.Succeeded)
                return Json(new MessageData(true, "添加成功"));

            return Json(new MessageData(false, result.Errors.First()));
        }

        /// <summary>
        /// 删除角色下的用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [PermissionCheck(Permission = "角色管理")]
        [HttpPost]
        public async Task<JsonResult> DeleteUserRole(int userId, string roleName)
        {
            var user = await _userService.FindByIdAsync(userId);
            if (user == null)
                return Json(new MessageData(false, "用户不存在"));

            var role = await _roleService.FindByNameAsync(roleName);
            if (role == null)
                return Json(new MessageData(false, "角色不存在"));

            var result = await _userService.RemoveFromRoleAsync(userId, roleName);
            if (result.Succeeded)
            {
                return Json(new MessageData(true, "删除成功"));
            }

            return Json(new MessageData(false, result.Errors.First()));
        }

        /// <summary>
        /// 编辑角色权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [PermissionCheck(Permission = "角色管理")]
        [HttpGet]
        public ActionResult EditRolePermissions(int id)
        {
            Role role = _roleService.FindById(id);
            if (role == null)
                return HttpNotFound();

            ViewData["roleIdd"] = role.Id;
            ViewData["permissions"] = role.Permissions.Select(n => _permissionService.FindById(n.PermissionId));
            return View(_permissionService.Permissions.ToList());
        }

        /// <summary>
        /// 编辑角色权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="permissionIds"></param>
        /// <returns></returns>
        [PermissionCheck(Permission = "角色管理")]
        [HttpPost]
        public ActionResult EditRolePermissions(int id, IEnumerable<int> permissionIds)
        {
            Role role = _roleService.FindById(id);
            if (role == null)
                return HttpNotFound();

            try
            {
                _permissionService.ClearRolePermissions(id);
                _permissionService.AddPermissionsToRole(id, permissionIds);
                return Json(new MessageData(true, "保存成功"));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new MessageData(false, ex.Message));
            }
        }

        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ActionResult _SearchUser(string keyword)
        {
            var users = _userService.Users.Where(n => n.TrueName.Contains(keyword) && !n.IsDelete);
            return View(users);
        }

        #endregion

        #region 菜单管理√

        /// <summary>
        /// 菜单管理
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult ManageNavigation(string searchInputs = null, int pageIndex = 1, int pageSize = 20)
        {
            var list = new PagedList<Navigation>(_navigationService.Navigations.OrderByDescending(n => n.Id).WhereIf(!String.IsNullOrEmpty(searchInputs), n => n.Name.ToLower().Contains(searchInputs.Trim().ToLower())), pageIndex, pageSize);
            return View(list);
        }

        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditNavigation(int? id = null)
        {
            Navigation navigation;
            if (id == null)
                navigation = new Navigation { IsEnabled = true };
            else
                navigation = _navigationService.FindById(id.Value);

            if (navigation == null)
                return HttpNotFound();

            var parentList = _navigationService.Navigations.Where(n => n.ParentId == null && n.Id != id).ToList();
            ViewData["parentList"] = new SelectList(parentList, "Id", "Name");

            List<SelectListItem> selectItems = new List<SelectListItem>()
            {
                new SelectListItem(){Text="是", Value="true"},
                new SelectListItem(){Text="否", Value="false"}
            };

            ViewData["isEnabledList"] = new SelectList(selectItems.ToList(), "Value", "Text");

            return View(navigation.MapTo<NavigationEditModel>());
        }

        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditNavigation(NavigationEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new MessageData(false, "操作失败"));
            }
            Navigation navigation;

            if (model.Id > 0)
            {
                navigation = _navigationService.FindById(model.Id);
                if (navigation == null)
                    return Json(new MessageData(false, "导航不存在"));
                var nameExists = _navigationService.Navigations.FirstOrDefault(n => n.Id != model.Id && n.Name == model.Name) != null;
                if (nameExists)
                    return Json(new MessageData(false, "导航名称已存在"));
                var permissionExists = _navigationService.Navigations.FirstOrDefault(n => n.Id != model.Id && n.PermissionName == model.PermissionName) != null;
                if (permissionExists)
                    return Json(new MessageData(false, "权限名称已存在"));
                navigation.ParentId = model.ParentId;
                navigation.Name = model.Name;
                navigation.RouteName = model.RouteName;
                navigation.Url = model.Url;
                navigation.PermissionName = model.PermissionName;
                navigation.IconName = model.IconName;
                navigation.DisplayOrder = model.DisplayOrder;
                navigation.IsEnabled = model.IsEnabled;
                try
                {
                    _navigationService.Update(navigation);
                    //改变权限，保持一致
                    Permission permission = _adminService.FindPermissionByName(navigation.PermissionName);
                    if (permission == null)
                        return Json(new MessageData(true, "编辑成功,角色权限修改失败"));
                    if (permission.ParentId.HasValue)
                    {
                        Navigation parentNavigation = _adminService.FindNavigationById(navigation.ParentId.Value);
                        Permission parentPermission = _adminService.FindPermissionByName(parentNavigation.PermissionName);
                        permission.ParentId = parentPermission.Id;
                        _adminService.UpdatePermission(permission);
                    }
                    return Json(new MessageData(true, "编辑成功"));
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return Json(new MessageData(false, "编辑失败，请重新编辑"));
                }
            }
            else
            {
                if (_navigationService.IsExists(model.Name))
                {
                    return Json(new MessageData(false, "导航名称已存在"));
                }
                var permissionExists = _navigationService.Navigations.FirstOrDefault(n => n.Id != model.Id && n.PermissionName == model.PermissionName) != null;
                if (permissionExists)
                    return Json(new MessageData(false, "权限名称已存在"));
                //添加权限
                Permission permission = new Permission { Name = model.PermissionName, ParentId = model.ParentId, Description = model.PermissionName, IsBuiltIn = true, IsButton = false, RolePermissonEnumType = RolePermissonEnum.Navigation };
                _adminService.InsertPermission(permission);
                //添加用户权限关联
                var userId = UserContext.CurrentUserId;
                _permissionService.AddPermissionToUser(userId, permission.Id);
                navigation = new Navigation()
                {
                    ParentId = model.ParentId,
                    Name = model.Name,
                    RouteName = model.RouteName,
                    Url = model.Url,
                    PermissionName = model.PermissionName,
                    IconName = model.IconName,
                    DisplayOrder = model.DisplayOrder,
                    IsEnabled = model.IsEnabled,
                };
                try
                {
                    _navigationService.Create(navigation);
                    return Json(new MessageData(true, "添加成功"));
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return Json(new MessageData(false, "添加失败"));
                }
            }
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteNavigation(int id)
        {
            var navigation = _navigationService.FindById(id);
            if (navigation == null)
                return Json(new MessageData(false, "信息不存在"));
            navigation.IsEnabled = false;
            try
            {
                _navigationService.Delete(navigation);
                return Json(new MessageData(true, "删除成功"));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new MessageData(false, "删除失败"));
            }
        }

        #endregion

        #region Common Area√


        /// <summary>
        /// 后台首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 后台首页局部页
        /// </summary>
        /// <returns></returns>
        public ActionResult _Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult _Header()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult _AsideMenu()
        {
            IList<Navigation> navigations = _navigationService.GetRootNavigations();
            return View(navigations);
        }


        [ChildActionOnly]
        public ActionResult _Footer()
        {
            return View();
        }

        #endregion

        #region 用户

        /// <summary>
        /// 用户管理
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [PermissionCheck(Permission = "用户管理")]
        public ActionResult ManageUsers(string searchInputs, UserType? userType = null, int pageIndex = 1, int pageSize = 20, int schoolAreaId = 0)
        {
            var currentUser = UserContext.CurrentUser;
            ViewData["userType"] = UserType.Admin.ToSelectListItemDIY("--用户类型--");
            var list = new PagedList<User>(_userService.GetUsers
                    .WhereIf(!String.IsNullOrEmpty(searchInputs), n => n.TrueName.ToLower().Contains(searchInputs.Trim().ToLower()) || n.UserName.ToLower().Contains(searchInputs.Trim().ToLower()) || (String.IsNullOrEmpty(n.PhoneNumber) ? false : n.PhoneNumber.ToLower().Contains(searchInputs.Trim().ToLower())))
                    .WhereIf(userType.HasValue, n => n.UserType == userType)
                    , pageIndex, pageSize);
            ViewData["searchInput"] = searchInputs;
            return View(list);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteUser(List<int> userId)
        {
            if (userId == null)
                return Json(new MessageData(false, "删除失败"));

            foreach (var id in userId)
            {
                User user = _userService.FindById(id);
                user.IsDelete = true;
                var result = _userService.Update(user);
            }

            return Json(new MessageData(true, "删除成功"));
        }


        /// <summary>
        /// 设为黑名单用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FreezeUser(List<int> userId)
        {
            if (userId == null)
                return Json(new MessageData(false, "设为失败"));

            foreach (var id in userId)
            {
                User user = _userService.FindById(id);
                user.IsBlackUser = true;
                if (user != null)
                {
                    var result = _userService.Update(user);
                }
            }

            return Json(new MessageData(true, "设为黑名单成功"));
        }

        /// <summary>
        /// 解除黑名单用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RefreezeUser(List<int> userId)
        {
            if (userId == null)
                return Json(new MessageData(false, "设为失败"));

            foreach (var id in userId)
            {
                User user = _userService.FindById(id);
                if (user != null && user.IsBlackUser)
                {
                    user.IsBlackUser = false;
                    var result = _userService.Update(user);
                }
            }

            return Json(new MessageData(true, "解除黑名单成功"));
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateUser(UserCreateModel model)
        {
            var currentUser = UserContext.CurrentUser;
            ViewData["UserType"] = UserType.Customer.ToSelectListItemNoEmpty();
            if (currentUser == null)
            {
                return HttpNotFound();
            }
            model.Roles = _roleService.Roles.ToList();
            return View(model);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateUser(UserCreateModel model, string[] role)
        {
            if (ModelState.IsValid)
            {
                if (_userService.FindByName(model.LoginAccount) != null)
                {
                    AddError("登录账户已存在");
                    return Json(new MessageData(false, "登录账户已存在"));
                }

                var user = new User();
                user.TrueName = model.UserName;
                user.UserName = model.LoginAccount;
                user.Password = model.PasswordModify;
                user.PhoneNumber = model.PhoneNumber;
                user.Description = model.Description;
                user.UserType = model.UserType;
                user.IsDelete = false;
                user.CreateTime = DateTime.Now;
                try
                {
                    var result = await _userService.CreateAsync(user, model.PasswordModify);
                    if (result.Succeeded)
                    {
                        if (role != null)
                        {
                            result = await _userService.AddToRolesAsync(user.Id, role);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return Json(new MessageData(false, "创建失败" + ex.Message));
                }
            }

            return Json(new MessageData(true, "创建成功"));
        }

        private void AddError(string message)
        {
            ViewData["errorMessage"] = message;
        }

        #endregion

        #region 省市区维护

        /// <summary>
        /// 导入省市区资料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InputArea()
        {


            return View();
        }

        public JsonResult InputArea(string jsonStr)
        {
            var areaEntity = new { Code = 0, Name = string.Empty };
            List<ChinaArea> list = new List<ChinaArea>();

            try
            {
                //areaEntity = JsonHelper.DeserializeAnonymousType(jsonStr, areaEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Json(new MessageData(true, "导入成功"));
        }

        #endregion

        #region 定时任务

        /// <summary>
        /// 管理定时任务
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult ManageTask(int pageIndex = 1, int pageSize = 20)
        {
            var list = new PagedList<QuartzTask>(_quartzTaskService.QuartzTasks, pageIndex, pageSize);
            return View(list);
        }

        /// <summary>
        /// 编辑任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditTask(int? id = null)
        {
            QuartzTask quartzTask = null;

            if (id.HasValue)
                quartzTask = _quartzTaskService.FindById(id.Value);
            else
            {
                quartzTask = new QuartzTask();
                quartzTask.Enabled = true;
                quartzTask.StartDate = DateTime.Now;
                quartzTask.TaskRule = "* * * * * ?";
            }

            if (quartzTask == null)
                return HttpNotFound();

            var classTypes = DIContainer.Resolve<ITypeFinder>().FindClassesOfType<ITask>();
            ViewData["classType"] = classTypes.Select(n => new SelectListItem { Text = n.Name, Value = n.FullName, Selected = n.FullName == quartzTask.ClassType });
            QuartzTaskEditModel editModel = quartzTask.AsEditModel();
            InitRules(editModel);
            return View(editModel);
        }

        /// <summary>
        /// 编辑任务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditTask(QuartzTaskEditModel model)
        {
            if (!ModelState.IsValid)
                return Json(new MessageData(false, "参数错误！"));

            if (model.Id == 0 && _quartzTaskService.QuartzTasks.Any(n => n.ClassType == model.ClassType))
                return Json(new MessageData(false, "任务已存在！"));

            try
            {
                QuartzTask task = model.AsQuartzTask();
                if (model.Id > 0)
                {
                    _quartzTaskService.Update(task);
                    DIContainer.Resolve<ITaskScheduler>().SaveTaskStatus(task);
                }
                else
                {
                    _quartzTaskService.Create(task);
                    DIContainer.Resolve<ITaskScheduler>().ResumeAll();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new MessageData(false, "提交失败！"));
            }

            return Json(new MessageData(true, "提交成功！"));
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteTask(List<int> id)
        {
            foreach (var idSingle in id)
            {
                var sub = _quartzTaskService.FindById(idSingle);
                if (sub == null)
                {
                    return HttpNotFound();
                }
            }
            return Json(new MessageData(true, "删除成功"));
        }

        /// <summary>
        /// 运行任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RunTask(int id)
        {
            DIContainer.Resolve<ITaskScheduler>().Run(id);
            return Json(new MessageData(true, "操作成功"));
        }

        /// <summary>
        /// 初始化任务规则
        /// </summary>
        private void InitRules(QuartzTaskEditModel editModel)
        {
            List<SelectListItem> seconds = new List<SelectListItem>();
            List<SelectListItem> minutes = new List<SelectListItem>();
            List<SelectListItem> hours = new List<SelectListItem>();
            List<SelectListItem> mouth = new List<SelectListItem>();
            List<SelectListItem> day = new List<SelectListItem>();
            List<SelectListItem> dayOfMouth = new List<SelectListItem>();

            for (int i = 0; i < 60; i++)
            {
                seconds.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = editModel.Seconds == i.ToString() });
                minutes.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = editModel.Minutes == i.ToString() });
                if (i > 0 && i <= 23)
                    hours.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = editModel.Hours == i.ToString() });

                if (i > 0 && i <= 12)
                    mouth.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = editModel.Mouth == i.ToString() });

                if (i > 0 && i <= 31)
                {
                    day.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = editModel.Day == i.ToString() });
                    dayOfMouth.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = editModel.DayOfMouth == i.ToString() });
                }
            }

            ViewData["Seconds"] = seconds;
            ViewData["Minutes"] = minutes;
            ViewData["Hours"] = hours;
            ViewData["Mouth"] = mouth;
            ViewData["Day"] = day;
            ViewData["DayOfMouth"] = dayOfMouth;

            ViewData["Frequency"] = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "每天", Value  = ((int)TaskFrequency.EveryDay).ToString(), Selected = TaskFrequency.EveryDay == editModel.Frequency },
                new SelectListItem(){ Text = "每周", Value  = ((int)TaskFrequency.Weekly).ToString(), Selected = TaskFrequency.Weekly == editModel.Frequency },
                new SelectListItem(){ Text = "每月", Value  = ((int)TaskFrequency.PerMonth).ToString(), Selected = TaskFrequency.PerMonth == editModel.Frequency  }
            };
            ViewData["Number"] = new List<SelectListItem>() {
                new SelectListItem(){ Text = "第一周", Value  = "1",Selected = editModel.Number == "1" },
                new SelectListItem(){ Text = "第二周", Value  = "2",Selected = editModel.Number == "2" },
                new SelectListItem(){ Text = "第三周", Value  = "3",Selected = editModel.Number == "3" },
                new SelectListItem(){ Text = "第四周", Value  = "4",Selected = editModel.Number == "4" }

            };

            ViewData["DayOfWeek"] = new Dictionary<string, string>() { { "周一", "2" }, { "周二", "3" }, { "周三", "4" }, { "周四", "5" }, { "周五", "6" }, { "周六", "7" }, { "周日", "1" } };
            ViewData["WeeklyOfMouth"] = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "周一", Value  = "2", Selected = editModel.WeeklyOfMouth == "2" },
                new SelectListItem(){ Text = "周二", Value  = "3", Selected = editModel.WeeklyOfMouth == "3" },
                new SelectListItem(){ Text = "周三", Value  = "4", Selected = editModel.WeeklyOfMouth == "4" },
                new SelectListItem(){ Text = "周四", Value  = "5", Selected = editModel.WeeklyOfMouth == "5" },
                new SelectListItem(){ Text = "周五", Value  = "6", Selected = editModel.WeeklyOfMouth == "6" },
                new SelectListItem(){ Text = "周六", Value  = "7", Selected = editModel.WeeklyOfMouth == "7" },
                new SelectListItem(){ Text = "周日", Value  = "1", Selected = editModel.WeeklyOfMouth == "1" }
            };
        }

        #endregion
    }
}