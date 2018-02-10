using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace hum_webapi.DomainObjects
{
    public class HumDataContext : DbContext
    {
        public DbSet<TaskItem> TaskItem { get; set; }
        public DbSet<TaskHistoryItem> TaskHistoryItem { get;set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=../hum.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TaskItem>()
                .HasMany(x => x.TaskHistory)
                .WithOne(x => x.Task);
        }
    }
}
