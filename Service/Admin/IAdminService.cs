using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    /// <summary>
    /// 系统管理接口
    /// </summary>
    public interface IAdminService
    {
        #region 角色权限

        /// <summary>
        /// 根据权限名获取权限
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Permission FindPermissionByName(string name);

        /// <summary>
        /// 根据权限名获取权限
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Navigation FindNavigationById(int id);

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        void UpdatePermission(Permission permission);

        /// <summary>
        /// 插入权限
        /// </summary>
        /// <param name="permission"></param>
        void InsertPermission(Permission permission);

        #endregion
    }
}
