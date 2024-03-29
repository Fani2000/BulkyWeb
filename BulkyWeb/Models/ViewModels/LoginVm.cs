using System.ComponentModel.DataAnnotations;

namespace Web.Models.ViewModels
{
    public class LoginVm
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public string? RedirectUrl { get; set; }
    }
}
