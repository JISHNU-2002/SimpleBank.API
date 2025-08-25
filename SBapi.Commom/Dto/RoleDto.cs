using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBapi.Common.Dto
{
    public class RoleDto
    {
        [MaxLength(100)]
        public required string RoleName { get; set; }
    }
}
