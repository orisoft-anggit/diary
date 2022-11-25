namespace Diary.DTOs
{
    public class GetDiaryDTO
    {
        public int Id {get; set;}

        public string Note {get; set;}

        public string Tittle {get; set;}

        public string Content {get; set;}

        public bool isArchieved {get; set;}

        public DateTime createAt {get; set;}

        public DateTime updateAt {get; set;}
    }
}