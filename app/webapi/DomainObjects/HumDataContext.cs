using System;
using Microsoft.EntityFrameworkCore;


namespace hum_webapi.DomainObjects
{
    public class HumDataContext : DbContext
    {
        public DbSet<TaskItem> TaskItem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=../hum.db");
        }
    }
}
