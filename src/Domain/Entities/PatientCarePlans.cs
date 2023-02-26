using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PatientCarePlans : BaseEntity
    { 
         
        public string? Title { get; set; } 
        public string? UserName { get; set; } 
        public string? PatientName { get; set; } 
        public string? Action { get; set; } 
        public string? Reason { get; set; } 
        public bool Completed { get; set; } 
        public string? Outcome { get; set; }
        public DateTime TargetStartDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; } 
    }
}
