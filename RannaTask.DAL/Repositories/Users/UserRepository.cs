using RannaTask.DAL.Contexts;
using RannaTask.DAL.Repositories.Customers;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.DAL.Repositories.Users;

public class UserRepository : GenericRepository<User, int>, IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
