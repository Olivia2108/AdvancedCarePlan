using System.ComponentModel.DataAnnotations;
using WebUI.Models.ViewModels;

namespace WebUI.Models.Dto
{
    public class CarePlanWebDto
    { 
        [Required]
        public long Id { get; set; }
        [Required]
        public string? Title { get; set; }
         
        [Required]
        public string? UserName { get; set; }
         
        [Required]
        public string? PatientName { get; set; }

        [StringLength(1000)]
        [Required]
        public string? Reason { get; set; }
        public bool Completed { get; set; }
         
        public string? Outcome { get; set; }
         
        [Required]
        public string? Action { get; set; }
        [Required]
        public DateTime TargetStartDate { get; set; }
        [Required]
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        [Required]
        public string? IpAddress { get; set; }

        public List<CarePlanWebVM>? List { get; set; }
    }
}
