using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
    }
}
