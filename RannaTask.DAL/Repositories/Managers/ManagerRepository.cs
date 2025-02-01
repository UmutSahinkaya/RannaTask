using RannaTask.DAL.Contexts;
using RannaTask.Entities.Entities;

namespace RannaTask.DAL.Repositories.Managers
{
    public class ManagerRepository : GenericRepository<Manager, int>,IManagerRepository
    {
        private readonly AppDbContext _context;
        public ManagerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }

}
