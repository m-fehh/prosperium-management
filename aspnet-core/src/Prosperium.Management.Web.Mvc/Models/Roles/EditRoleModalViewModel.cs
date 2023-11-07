using Abp.AutoMapper;
using Prosperium.Management.Roles.Dto;
using Prosperium.Management.Web.Models.Common;

namespace Prosperium.Management.Web.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class EditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool HasPermission(FlatPermissionDto permission)
        {
            return GrantedPermissionNames.Contains(permission.Name);
        }
    }
}
