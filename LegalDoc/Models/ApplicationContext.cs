using Microsoft.EntityFrameworkCore;
using LegalDoc.Models;

namespace LegalDoc.Models
{
    [System.Data.Entity.DbConfigurationType(typeof(MySql.Data.EntityFramework.MySqlEFConfiguration))]
    public class ApplicationContext : DbContext
    {
        public DbSet<Document> documents { get; set; } = null!;
        public DbSet<ImportantDocuments> importantDocuments { get; set; } = null!;
        public DbSet<User> users { get; set; } = null!;
        public DbSet<Role> roles { get; set; } = null!;


        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.Migrate();
        }


        public DbSet<LegalDoc.Models.SendingObj> SendingObj { get; set; }
    }
}
