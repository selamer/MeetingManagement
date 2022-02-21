using System.ComponentModel.DataAnnotations;

namespace MeetingManagement.ViewModel
{
    public class BuildingViewModel
    {
        public int Building_ID { get; set; }

        [Required, MaxLength(50), Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Image")]
        public string Image { get; set; }
        public IFormFile PhotoFile { get; set; }

        [Required, MaxLength(100), Display(Name = "Location")]
        public string Location { get; set; }
    }
}
