using System.ComponentModel.DataAnnotations;

namespace User.Management.API.Models.CRUD
{
    public class UpdateUserModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? NewUsername { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? NewEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "2auth param is required")]
        public bool NewTwoFactorEnabled { get; set; }

        /*[Required(ErrorMessage = "Role is required ")]
        public bool? NewRole { get; set; }*/
    }
}
