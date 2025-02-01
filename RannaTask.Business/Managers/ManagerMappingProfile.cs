using AutoMapper;
using RannaTask.Business.Customers;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Managers
{
    public class ManagerMappingProfile : Profile
    {
        public ManagerMappingProfile()
        {
            CreateMap<Manager, CreateManagerDto>().ReverseMap();
            CreateMap<Manager, ManagerDto>().ReverseMap();
        }
    }
}
