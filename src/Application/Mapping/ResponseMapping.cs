using Application.ViewModels;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping
{
    public class ResponseMapping : Profile
    {
        public ResponseMapping()
        {
            CreateMap<PatientCarePlan, PatientCarePlanVM>();
        }
    }
}
