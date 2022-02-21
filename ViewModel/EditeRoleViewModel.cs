using System.ComponentModel.DataAnnotations;

namespace MeetingManagement.ViewModel
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }

        public string Id { get; set; }

        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Role Name Is Required")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
