using Common;
using Common.Utilities;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    /// <summary>
    /// 权限业务逻辑接口
    /// </summary>
    public interface IPermissionService
    {

        IQueryable<Permission> Permissions
        {
            get;
        }

        /// <summary>
        /// 根据Id获取权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Permission FindById(int id);

        /// <summary>
        /// 根据权限名称获取权限
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Permission FindByName(string name);

        /// <summary>
        /// 检查权限是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsExists(string name);

        /// <summary>
        /// 创建权限
        /// </summary>
        /// <param name="permission"></param>
        void Create(Permission permission);

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="permission"></param>
        void Update(Permission permission);

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="permission"></param>
        void Delete(Permission permission);

        /// <summary>
        /// 获取角色的权限集合
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        IList<Permission> GetRolePermissions(Role role);

        /// <summary>
        /// 获取角色的权限集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        IList<Permission> GetRolePermissions(int roleId);

        /// <summary>
        /// 获取用户的权限集合
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IList<Permission> GetUserPermissions(User user);

        /// <summary>
        /// 获取角色的权限集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IList<Permission> GetUserPermissions(int userId);

        /// <summary>
        /// 添加权限到指定角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionId"></param>
        void AddPermissionToRole(int roleId, int permissionId, RolePermissonEnum rolePermissonEnum);

        /// <summary>
        /// 批量添加权限到指定角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionIds"></param>
        void AddPermissionsToRole(int roleId, IEnumerable<int> permissionIds,RolePermissonEnum? rolePermissonEnum=null);

        /// <summary>
        /// 删除角色下的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionId"></param>
        void DeleteRolePermission(int roleId, int permissionId);

        /// <summary>
        /// 清空角色权限
        /// </summary>
        /// <param name="roleId"></param>
        void ClearRolePermissions(int roleId, RolePermissonEnum? rolePermissonEnum=null);

        /// <summary>
        /// 添加权限到指定用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissionId"></param>
        void AddPermissionToUser(int userId, int permissionId);

        /// <summary>
        /// 批量添加权限到指定用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissionIds"></param>
        void AddPermissionsToUser(int userId, IEnumerable<int> permissionIds);

        /// <summary>
        /// 删除用户下的权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissionId"></param>
        void DeleteUserPermission(int userId, int permissionId);

        /// <summary>
        /// 清空用户权限
        /// </summary>
        /// <param name="userId"></param>
        void ClearUserPermissions(int userId);
    }
}
