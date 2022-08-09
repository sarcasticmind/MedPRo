using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace keef2.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Not Matched with the password")]
        public string ConfirmPassword { get; set; }
    }
}
