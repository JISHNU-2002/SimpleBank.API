using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBapi.Common.Dto
{
    public class UserRolesDto
    {
        public int FormId { get; set; }
        public required string UserName { get; set; }
        public required string AccountNumber { get; set; }
    }
}
