
using System.ComponentModel.DataAnnotations;

namespace Code2.ViewModel
{
    public class AddRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
