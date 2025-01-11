using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using quartz.entity;

namespace quartz.db
{
    public class QuartzDbContext : DbContext
    {
        public required DbSet<ProductionCapacity> ProductionCapacities { get; set; }
        public required DbSet<QuartzSchedule> QuartzSchedules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Env.GetString("DATABASE_URL");

            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
