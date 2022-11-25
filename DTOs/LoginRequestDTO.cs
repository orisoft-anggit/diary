using System.ComponentModel.DataAnnotations;

namespace Diary.DTOs
{
    public class LoginRequestDTO
    {
        [Display(Name = "UserName")]
        [Required(ErrorMessage = "you must fill the username")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Username is not valid")]
        [StringLength(16, ErrorMessage = "Must be between 3 and 16 characters", MinimumLength = 3)]
        public string UserName {get; set;}

        [Display(Name = "Email")]
        [Required(ErrorMessage = "you must fill the email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string Email {get; set;}

        [Required(ErrorMessage = "you must fill the password")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password {get; set;}

        public LoginRequestDTO()
        {

        }
    }
}