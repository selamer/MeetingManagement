using System.ComponentModel.DataAnnotations;

namespace MeetingManagement.ViewModel
{
    public class VisitorViewModel
    {
        public int Book_Id { get; set; }

        [Required, Display(Name = "Name")]
        [RegularExpression(@"([A-Za-z])+( [A-Za-z]+)", ErrorMessage = "Must Be Only Characters..!")]
        public string Name { get; set; }

        [EmailAddress, Display(Name = "Email")]
        public string? Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [Display(Name = "National ID")]
        public string SSN { get; set; }

        [Display(Name = "Photo Of SSN")]
        public string? PhotoSSN { get; set; }
        public IFormFile? ImageSSN { get; set; }

        [Display(Name = "Employer")]
        public string? Employer { get; set; }

        [Display(Name = "Job Title")]
        public string? JobTitle { get; set; }

        public string? Nationality { get; set; }

        [Display(Name = "Vaccine Number")]
        public string? Vaccine { get; set; }

        [Display(Name = "Photo Vaccine")]
        public string? PhotoVaccine { get; set; }

        public IFormFile? ImageVaccine { get; set; }
    }
}
