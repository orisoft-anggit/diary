using Diary.Models;
using Microsoft.EntityFrameworkCore;

namespace Diary.Data
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions options) : base(options)
        {

        }
        
        public DbSet<AuthEntity> AuthEntities { get; set; }
        public DbSet<DiaryEntity> DiaryEntitys {get; set;}
    }
}
