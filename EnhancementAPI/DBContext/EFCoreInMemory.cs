using EnhancementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EnhancementAPI.DBContext
{
    public class EFCoreInMemory
    {
        public class ApiContext : DbContext
        {
            protected override void OnConfiguring
           (DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseInMemoryDatabase(databaseName: "TaskDb");
            }
            public DbSet<UserDetails> UserDetails { get; set; }
            public DbSet<TaskDetails> TaskDetails { get; set; }
        }
    }
}
