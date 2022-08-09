using System.ComponentModel.DataAnnotations;

namespace keef2.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool isPresistant { get; set; }

    }
}
