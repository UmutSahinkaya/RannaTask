using RannaTask.DAL.Contexts;
using RannaTask.Entities.Entities;

namespace RannaTask.DAL.Repositories.Customers;

public class CustomerRepository : GenericRepository<Customer, int>, ICustomerRepository
{
    private readonly AppDbContext _context;
    public CustomerRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}