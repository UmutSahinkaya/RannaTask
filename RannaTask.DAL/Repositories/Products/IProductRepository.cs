using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.DAL.Repositories.Products
{
    public interface IProductRepository : IGenericRepository<Product, int>
    {
        IQueryable<Product> GetAllWithCreator();
    }
}
