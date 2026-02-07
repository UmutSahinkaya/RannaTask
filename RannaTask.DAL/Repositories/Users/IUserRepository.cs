using RannaTask.DAL.Contexts;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.DAL.Repositories.Users;

public interface IUserRepository : IGenericRepository<User, int>
{
}
