using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Customers
{
    public class CreateCustomerResponse
    {
        public CreateCustomerResponse(int ıd)
        {
            Id = ıd;
        }

        public CreateCustomerResponse(string message)
        {
            Message = message;
        }

        public int Id { get; set; }
        public string Message { get; set; }
    }
}
