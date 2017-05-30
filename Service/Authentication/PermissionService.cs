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
    /// 权限业务逻辑
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IRepository<RolePermission> _rolePermissionRepository;
        private readonly IRepository<UserPermission> _userPermissionRepository;

        public PermissionService(IRepository<Permission> permissionRepository, IRepository<RolePermission> rolePermissionRepository, IRepository<UserPermission> userPermissionRepository)
        {
            this._permissionRepository = permissionRepository;
            this._rolePermissionRepository = rolePermissionRepository;
            this._userPermissionRepository = userPermissionRepository;
        }

        public virtual IQueryable<Permission> Permissions
        {
            get
            {
                return _permissionRepository.Table;
            }
        }

        /// <summary>
        /// 根据Id获取权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Permission FindById(int id)
        {
            return _permissionRepository.GetById(id);
        }

        /// <summary>
        /// 根据权限名称获取权限
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Permission FindByName(string name)
        {
            return _permissionRepository.FirstOrDefault(n => n.Name.Equals(name));
        }

        /// <summary>
        /// 检查权限是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsExists(string name)
        {
            if (_permissionRepository.FirstOrDefault(n => n.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) == null)
                return false;
            return true;
        }

        /// <summary>
        /// 创建权限
        /// </summary>
        /// <param name="permission"></param>
        public void Create(Permission permission)
        {
            _permissionRepository.Insert(permission);
        }

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="permission"></param>
        public void Update(Permission permission)
        {
            _permissionRepository.Update(permission);
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="permission"></param>
        public void Delete(Permission permission)
        {
            _permissionRepository.Delete(permission);
            var list = from r in _rolePermissionRepository.Table
                       where r.PermissionId == permission.Id
                       select r;
            _rolePermissionRepository.Delete(list.ToList());
        }

        /// <summary>
        /// 获取角色的权限集合
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public IList<Permission> GetRolePermissions(Role role)
        {
            return GetRolePermissions(role.Id);
        }

        /// <summary>
        /// 获取角色的权限集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public IList<Permission> GetRolePermissions(int roleId)
        {
            var permissions = from p in _permissionRepository.Table
                              join r in _rolePermissionRepository.Table
                              on p.Id equals r.PermissionId
                              where r.RoleId == roleId
                              select p;
            return permissions.ToList();
        }

        /// <summary>
        /// 获取用户的权限集合
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IList<Permission> GetUserPermissions(User user)
        {
            return GetUserPermissions(user.Id);
        }

        /// <summary>
        /// 获取用户的权限集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Permission> GetUserPermissions(int userId)
        {
            var permissions = from p in _permissionRepository.Table
                              join r in _userPermissionRepository.Table
                              on p.Id equals r.PermissionId
                              where r.UserId == userId
                              select p;
            return permissions.ToList();
        }

        /// <summary>
        /// 添加权限到指定角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionId"></param>
        public void AddPermissionToRole(int roleId, int permissionId, RolePermissonEnum rolePermissonEnum)
        {
            RolePermission rolePermission = new RolePermission { RoleId = roleId, PermissionId = permissionId ,RolePermissonEnumType=rolePermissonEnum};
            _rolePermissionRepository.Insert(rolePermission);
        }

        /// <summary>
        /// 批量添加权限到指定角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionIds"></param>
        public void AddPermissionsToRole(int roleId, IEnumerable<int> permissionIds, RolePermissonEnum? rolePermissonEnum=null)
        {
            if (permissionIds != null)
            {
                foreach (var permissionId in permissionIds)
                {
                    RolePermission rolePermission = new RolePermission { RoleId = roleId, PermissionId = permissionId, RolePermissonEnumType = rolePermissonEnum.Value};
                    _rolePermissionRepository.Insert(rolePermission);
                }
            }
        }

        /// <summary>
        /// 删除角色下的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionId"></param>
        public void DeleteRolePermission(int roleId, int permissionId)
        {
            var rolePermission = _rolePermissionRepository.Table.Where(n => n.RoleId == roleId && n.PermissionId == permissionId).FirstOrDefault();
            _rolePermissionRepository.Delete(rolePermission);
        }

        /// <summary>
        /// 清空角色权限
        /// </summary>
        /// <param name="roleId"></param>
        public void ClearRolePermissions(int roleId, RolePermissonEnum? rolePermissonEnum=null)
        {
            var rolePermissions = from r in _rolePermissionRepository.Table
                                  where r.RoleId == roleId
                                  where r.RolePermissonEnumType==rolePermissonEnum
                                  select r;
            _rolePermissionRepository.Delete(rolePermissions.ToList());
        }

        /// <summary>
        /// 添加权限到指定用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissionId"></param>
        public void AddPermissionToUser(int userId, int permissionId)
        {
            var permission = _userPermissionRepository.FirstOrDefault(n => n.UserId == userId && n.PermissionId == permissionId);
            if (permission != null)
                return;

            UserPermission userPermission = new UserPermission
            {
                UserId = userId,
                PermissionId = permissionId
            };
            _userPermissionRepository.Insert(userPermission);
        }

        /// <summary>
        /// 批量添加权限到指定用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissionIds"></param>
        public void AddPermissionsToUser(int userId, IEnumerable<int> permissionIds)
        {
            if (permissionIds != null)
            {
                foreach (var permissionId in permissionIds)
                {
                    AddPermissionToUser(userId, permissionId);
                }
            }
        }

        /// <summary>
        /// 删除用户下的权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissionId"></param>
        public void DeleteUserPermission(int userId, int permissionId)
        {
            var userPermission = _userPermissionRepository.Table.Where(n => n.UserId == userId && n.PermissionId == permissionId).FirstOrDefault();
            _userPermissionRepository.Delete(userPermission);
        }

        /// <summary>
        /// 清空用户权限
        /// </summary>
        /// <param name="userId"></param>
        public void ClearUserPermissions(int userId)
        {
            var userPermissions = from u in _userPermissionRepository.Table
                                  where u.UserId == userId
                                  select u;
            _userPermissionRepository.Delete(userPermissions.ToList());
        }
    }
}
