using RannaTask.DAL.Contexts;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.DAL.Repositories.Products
{
    public class ProductRepository : GenericRepository<Product, int>, IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
