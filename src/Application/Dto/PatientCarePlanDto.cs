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
		public string? Title { get; set; }

		[StringLength(450)]
		public string? UserName { get; set; }

		[StringLength(450)]
		public string? PatientName { get; set; } 

		[StringLength(1000)]
		public string? Reason { get; set; }
		public bool Completed { get; set; }

		[StringLength(1000)]
		public string? Outcome { get; set; }

		[StringLength(1000)]
		public string? Action { get; set; }
		public DateTime TargetStartDate { get; set; }
		public DateTime ActualStartDate { get; set; }
		public DateTime ActualEndDate { get; set; }
		public string? IpAddress { get; set; }
	}
}
