using System.Collections.Generic;
using Prosperium.Management.Roles.Dto;

namespace Prosperium.Management.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
