using Common;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class RoleStore : IQueryableRoleStore<Role, int>, IRoleStore<Role, int>
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;

        public RoleStore(IRepository<Role> roleRepository, IRepository<UserRole, long> userRoleRepository)
        {
            this._roleRepository = roleRepository;
            this._userRoleRepository = userRoleRepository;
        }

        public virtual IQueryable<Role> Roles
        {
            get { return _roleRepository.Table; }
        }

        public virtual async Task CreateAsync(Role role)
        {
            await _roleRepository.InsertAsync(role);
        }

        public virtual async Task UpdateAsync(Role role)
        {
            await _roleRepository.UpdateAsync(role);
        }

        public virtual async Task DeleteAsync(Role role)
        {
            await _userRoleRepository.DeleteAsync(_userRoleRepository.Table.Where(n => n.RoleId == role.Id).ToList());
            await _roleRepository.DeleteAsync(role);
        }

        public virtual async Task<Role> FindByIdAsync(int roleId)
        {
            return await _roleRepository.FirstOrDefaultAsync(roleId);
        }

        public virtual async Task<Role> FindByNameAsync(string roleName)
        {
            return await _roleRepository.FirstOrDefaultAsync(
                role => role.Name == roleName
                );
        }

        public virtual void Dispose()
        {

        }
    }
}
