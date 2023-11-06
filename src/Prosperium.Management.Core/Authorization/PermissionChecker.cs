using Abp.Authorization;
using Prosperium.Management.Authorization.Roles;
using Prosperium.Management.Authorization.Users;

namespace Prosperium.Management.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
