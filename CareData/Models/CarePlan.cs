using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareData.Models
{
	public class CarePlan
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

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
		public string? IpAddress { get; set; }
		public bool IsActive { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateUpdated { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime DateDeleted { get; set; } 
	}
}
