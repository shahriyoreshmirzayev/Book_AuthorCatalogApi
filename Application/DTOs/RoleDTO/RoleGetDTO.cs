using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.RoleDTO
{
    public class RoleGetDTO
    {
        public int RoleId { get; set; }

        public string Name { get; set; }

        public int[] PermissionsId { get; set; }

    }
}
