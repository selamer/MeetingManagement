using MeetingManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace MeetingManagement.ViewModel
{
    public class RoomViewModel
    {
        public int ConferenceRoom_ID { get; set; }

        [Required, MaxLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Image")]
        public string Image { get; set; }
        public IFormFile PhotoFile { get; set; }

        [Required]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; }
        public int Building_ID { get; set; }
        public virtual Building Building { get; set; }
        public virtual List<int> Additions { get; set; }
        public List<AdditionsConferenceRoom> AdditionsConferenceRooms = new List<AdditionsConferenceRoom>();
    }
}
