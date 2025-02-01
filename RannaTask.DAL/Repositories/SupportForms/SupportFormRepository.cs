using RannaTask.DAL.Contexts;
using RannaTask.DAL.Repositories.Products;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.DAL.Repositories.SupportForms
{
    public class SupportFormRepository : GenericRepository<SupportForm, int>, ISupportFormRepository
    {
        private readonly AppDbContext _context;
        public SupportFormRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
