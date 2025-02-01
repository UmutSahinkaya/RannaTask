using RannaTask.DAL.Contexts;
using RannaTask.Entities.Entities;

namespace RannaTask.DAL.Repositories.UserRoles;

public class UserRoleRepository : GenericRepository<UserRole, int>, IUserRoleRepository
{
    private readonly AppDbContext _context;
    public UserRoleRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
