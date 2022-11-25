namespace Diary.Models
{
    public class DiaryEntity
    {
        public int Id {get; set;}

        public string Note {get; set;}

        public string Tittle {get; set;}

        public string Content {get; set;}

        public bool Is_Archieved {get; set;}

        public DateTime Create_At {get; set;}

        public DateTime Update_At {get; set;}

        public AuthEntity AuthEntity {get; set;}

    }
}