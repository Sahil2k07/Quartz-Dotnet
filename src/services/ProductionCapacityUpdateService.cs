using Quartz;
using quartz.db;
using quartz.entity;
using quartz.enums;

namespace quartz.services
{
    public class ProductionCapacityUpdateService(QuartzDbContext context) : IJob
    {
        private readonly QuartzDbContext _dbContext = context;
        private readonly string jobName = QuartzJobs.ProductionCapacityUpdate.ToString();

        public DateTime FetchTriggerStartTime()
        {
            var quartzSchedule = _dbContext
                .QuartzSchedules.Where(qs => qs.JobName == jobName)
                .FirstOrDefault();

            if (quartzSchedule == null)
            {
                var newQuartzSchedule = new QuartzSchedule
                {
                    JobName = jobName,
                    NextRunTime = DateTime.Today.AddDays(1).ToUniversalTime(),
                };

                _dbContext.QuartzSchedules.Add(newQuartzSchedule);
                _dbContext.SaveChanges();

                Console.WriteLine("New Quartz schedule added");

                return newQuartzSchedule.NextRunTime;
            }

            return quartzSchedule.NextRunTime;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            DateTime nextRunTime = FetchTriggerStartTime();

            Console.WriteLine($"Next run time was {nextRunTime}");

            if (nextRunTime > DateTime.UtcNow)
            {
                return;
            }

            UpdateNextTriggerTime(DateTime.Today.AddDays(30).ToUniversalTime());
            await Task.CompletedTask;
        }

        public void UpdateNextTriggerTime(DateTime time)
        {
            var quartzSchedule = _dbContext
                .QuartzSchedules.Where(qs => qs.JobName == jobName)
                .FirstOrDefault();

            quartzSchedule.NextRunTime = time;
            _dbContext.QuartzSchedules.Update(quartzSchedule);
            _dbContext.SaveChanges();

            Console.WriteLine($"Next run time updated successfully - {time}");
        }
    }
}
