using RannaTask.DAL.Repositories.Products;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.DAL.Repositories.Customers;

public interface ICustomerRepository : IGenericRepository<Customer, int>
{
}
