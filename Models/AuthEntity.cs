using System.Text.Json.Serialization;

namespace Diary.Models
{
    public class AuthEntity
    {
        public int Id { get; set;}

        public string UserName {get; set;}

        public string Email {get; set;}

        public byte[] PasswordSalt { get; set; }

        [JsonIgnore]
        public byte[] PasswordHash { get; set; }

        public List<DiaryEntity>DiaryEntitys {get; set;}

    }
}