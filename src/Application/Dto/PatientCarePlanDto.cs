using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
	public class PatientCarePlanDto
	{
		[StringLength(450)]
        [Required]
        public string? Title { get; set; }

		[StringLength(450)]
        [Required]
        public string? UserName { get; set; }

		[StringLength(450)]
        [Required]
        public string? PatientName { get; set; } 

		[StringLength(1000)]
		[Required]
		public string? Reason { get; set; }
		public bool Completed { get; set; }

		[StringLength(1000)]
		public string? Outcome { get; set; }

		[StringLength(1000)]
        [Required]
        public string? Action { get; set; }
        [Required]
        public DateTime TargetStartDate { get; set; }
        [Required]
        public DateTime ActualStartDate { get; set; }
		public DateTime ActualEndDate { get; set; }
        [Required]
        public string? IpAddress { get; set; }
	}
}
