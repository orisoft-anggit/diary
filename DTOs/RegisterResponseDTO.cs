using System.ComponentModel.DataAnnotations;

namespace Diary.DTOs
{
    public class RegisterResponseDTO
    {
        public int Id {get; set;}
        
        public string Email {get; set;}

        public string UserName {get; set;}

        public string Password {get; set;}

        public RegisterResponseDTO()
        {

        }

    }
}