using System.Collections.Generic;
using Prosperium.Management.Roles.Dto;

namespace Prosperium.Management.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}