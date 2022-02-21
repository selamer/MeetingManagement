using System.ComponentModel.DataAnnotations;

namespace MeetingManagement.ViewModel
{
    public class BookViewModel
    {
        public int ConferenceRoom_ID { get; set; }

        [Display(Name = "Date")]
        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Start Time")]
        [Required, DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [Display(Name = "End Time")]
        [Required, DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        [Display(Name = "Number Of Visitors")]
        [Required, Range(1, 50)]
        public int NumOfVisitors { get; set; }

        [Display(Name = "Internal Visitors")]
        public int InternalVisitors { get; set; }

        [Display(Name = "External Visitors")]
        public int ExternalVisitors { get; set; }

        [Display(Name = "Additions")]
        public List<int>? Additions { get; set; }
    }
}
