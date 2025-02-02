using AutoMapper;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.SupportForms
{
    public class SupportFormMappingProfile:Profile
    {
        public SupportFormMappingProfile()
        {
            CreateMap<SupportForm,SupportFormDto>().ReverseMap();
            CreateMap<SupportForm, CreateSupportFormDto>().ReverseMap();
        }
    }
}
