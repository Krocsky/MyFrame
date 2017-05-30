using Common;
using Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    /// <summary>
    /// 系统管理业务逻辑
    /// </summary>
    public class AdminService : IAdminService
    {
        private readonly EFDbContext _context;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IRepository<Navigation> _navigationRepository;

        public AdminService(EFDbContext context, IRepository<User> userRepository, IRepository<Permission> permissionRepository, IRepository<Navigation> navigationRepository)
        {
            this._context = context;
            this._userRepository = userRepository;
            this._permissionRepository = permissionRepository;
            this._navigationRepository = navigationRepository;
        }

        #region 角色管理

        /// <summary>
        /// 根据权限名获取权限
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Permission FindPermissionByName(string name)
        {
            Permission permission = _permissionRepository.Table.Where(n => n.Name == name).FirstOrDefault();
            return permission;
        }

        /// <summary>
        /// 根据权限名获取权限
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Navigation FindNavigationById(int id)
        {
            Navigation navigation = _navigationRepository.Table.Where(n => n.Id == id).FirstOrDefault();
            return navigation;
        }

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="permission">实体</param>
        /// <returns></returns>
        public void UpdatePermission(Permission permission)
        {
            _permissionRepository.Update(permission);
        }

        /// <summary>
        /// 插入权限
        /// </summary>
        /// <param name="permission"></param>
        public void InsertPermission(Permission permission)
        {
            _permissionRepository.Insert(permission);
        }

        #endregion
    }
}
