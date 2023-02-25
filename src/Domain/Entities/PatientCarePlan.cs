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
    public class PatientCarePlan : BaseEntity
    { 

        [Required]
        [MaxLength(450)]
        public string? Title { get; set; }

        [Required]
        [MaxLength(450)]
        public string? UserName { get; set; }

        [Required]
        [MaxLength(450)]
        public string? PatientName { get; set; }

        [Required]
        [MaxLength(1000)]
        public string? Action { get; set; }

        [Required]
        [MaxLength(1000)]
        public string? Reason { get; set; }

        public bool Completed { get; set; }

        [MaxLength(1000)]
        public string? Outcome { get; set; }
        public DateTime TargetStartDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; } 
    }
}
