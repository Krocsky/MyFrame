using Common;
using Common.Utilities;
using Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    /// <summary>
    /// 用户权限业务逻辑
    /// </summary>
    public class RoleService : RoleManager<Role, int>
    {
        public RoleService(IRoleStore<Role, int> roleStore)
            : base(roleStore)
        {
        }

        public static RoleService Create(IdentityFactoryOptions<RoleService> options, IOwinContext context)
        {
            return new RoleService(DIContainer.Resolve<RoleStore>());
        }

        public static List<Role> GetAllRoles(IdentityFactoryOptions<RoleService> options, IOwinContext context)
        {
            var roleService = new RoleService(DIContainer.Resolve<RoleStore>());
            return roleService.Roles.ToList();
        }
    }
}
