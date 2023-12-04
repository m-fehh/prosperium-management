using System.Collections.Generic;
using Prosperium.Management.MultiTenancy;
using Prosperium.Management.Roles.Dto;

namespace Prosperium.Management.Web.Models.Users
{
    public class UserListViewModel
    {
        public List<Tenant> Tenants { get; set; }
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
