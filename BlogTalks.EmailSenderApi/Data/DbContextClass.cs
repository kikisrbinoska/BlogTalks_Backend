using BlogTalks.EmailSenderApi.DTO;
using Microsoft.EntityFrameworkCore;

namespace BlogTalks.EmailSenderApi.Data
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbContextClass(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<EmailDTO> Emails { get; set; }
    }
}
