using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBapi.Common.Dto
{
    public class AddRemoveRoleDto
    {
        public List<AddRemoveRole> AddRemoveRoles { get; set; } = new();
        public required string UserId { get; set; }
        public required string UserName { get; set; }
    }
    public class AddRemoveRole
    {
        public required string RoleId { get; set; }
        public required string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
