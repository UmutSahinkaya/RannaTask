using AutoMapper;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Customers
{
    public class CustomerMappingProfile:Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer,CreateCustomerDto>().ReverseMap();
            CreateMap<Customer,CustomerDto>().ReverseMap();
        }
    }
}
