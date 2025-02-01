using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Helpers
{
    public interface ITokenService
    {
        string GenerateTokenUser(User user);
        string GenerateTokenCustomer(Customer customer);
    }
}
