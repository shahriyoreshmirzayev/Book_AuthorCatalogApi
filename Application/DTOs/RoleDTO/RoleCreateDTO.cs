using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.RoleDTO
{
    public class RoleCreateDTO
    {
        public string Name { get; set; }

        public int[] PermissionsId { get; set; }
    }
}
