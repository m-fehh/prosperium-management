using System.Collections.Generic;
using Prosperium.Management.Roles.Dto;

namespace Prosperium.Management.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
