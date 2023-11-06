using System.Collections.Generic;
using System.Linq;
using Prosperium.Management.Roles.Dto;
using Prosperium.Management.Users.Dto;

namespace Prosperium.Management.Web.Models.Users
{
    public class EditUserModalViewModel
    {
        public UserDto User { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }

        public bool UserIsInRole(RoleDto role)
        {
            return User.RoleNames != null && User.RoleNames.Any(r => r == role.NormalizedName);
        }
    }
}
