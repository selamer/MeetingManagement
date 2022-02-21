using System.ComponentModel.DataAnnotations;

namespace MeetingManagement.ViewModel
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Roles = new List<string>();
        }

        public string Id { get; set; }

        [Required, Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required, EmailAddress, Display(Name = "Email")]
        public string Email { get; set; }

        [Required, Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required, Display(Name = "User ID")]
        public string UserId { get; set; }

        public List<string> Roles { get; set; }
    }
}
