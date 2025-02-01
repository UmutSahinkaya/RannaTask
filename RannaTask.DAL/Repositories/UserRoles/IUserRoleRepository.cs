using RannaTask.DAL.Repositories.Managers;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.DAL.Repositories.UserRoles;

public interface IUserRoleRepository : IGenericRepository<UserRole, int>
{
}
