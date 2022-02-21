using System.ComponentModel.DataAnnotations;

namespace MeetingManagement.ViewModel
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        public bool IsSelected { get; set; }
    }
}