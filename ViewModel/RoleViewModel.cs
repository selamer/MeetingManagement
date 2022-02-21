using System.ComponentModel.DataAnnotations;

namespace MeetingManagement.ViewModel
{
    public class RoleViewModel
    {
        [Required, Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
