using Microsoft.AspNetCore.Mvc.Rendering;
using WebUI.Models.Dto;

namespace WebUI.Models.ViewModels
{
    public class CarePlanWebVM
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? UserName { get; set; }
        public string? PatientName { get; set; }
        public string? Reason { get; set; }
        public bool Completed { get; set; }
        public string? Outcome { get; set; }
        public string? Action { get; set; }
        public DateTime TargetStartDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
