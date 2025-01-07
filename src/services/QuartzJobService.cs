using Quartz;
using quartz.db;
using quartz.entity;

namespace quartz.services
{
    public class CapacityUpdateService : IJob
    {
        private readonly QuartzDbContext _dbContext;

        public CapacityUpdateService(QuartzDbContext context)
        {
            _dbContext = context;
        }

        public DateTime FetchTriggerStartTime()
        {
            var nextRunTime = _dbContext
                .QuartzSchedules.FirstOrDefault(qs => qs.JobName == "CapacityUpdateJob")
                ?.NextRunTime;

            if (!nextRunTime.HasValue)
            {
                nextRunTime = DateTime.UtcNow.AddSeconds(10);
                var newQuartzSchedule = new QuartzSchedule
                {
                    JobName = "CapacityUpdateJob",
                    NextRunTime = nextRunTime.Value,
                };

                _dbContext.QuartzSchedules.Add(newQuartzSchedule);
                _dbContext.SaveChanges();
                Console.WriteLine("Added a new QuartzSchedule Automatically");
            }

            return nextRunTime.Value.ToUniversalTime();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            DateTime nextRunTime = FetchTriggerStartTime();
            DateTime previousThreeMonths = nextRunTime.AddMinutes(-1);

            var productionCapacities = _dbContext
                .ProductionCapacities.Where(pc => pc.Day >= previousThreeMonths)
                .ToList();

            foreach (var capacity in productionCapacities)
            {
                var newCapacity = new ProductionCapacity
                {
                    Identifier = capacity.Identifier,
                    Day = capacity.Day.AddMonths(3),
                };
                _dbContext.ProductionCapacities.Add(newCapacity);
            }

            UpdateNextTriggerTime(nextRunTime.AddMinutes(1));

            Console.WriteLine("UpdateTriggered and done");
            await Task.CompletedTask;
        }

        public void UpdateNextTriggerTime(DateTime time)
        {
            var quartzSchedule = _dbContext.QuartzSchedules.FirstOrDefault(qs =>
                qs.JobName == "CapacityUpdateJob"
            );

            quartzSchedule.NextRunTime = time.ToUniversalTime();
            _dbContext.QuartzSchedules.Update(quartzSchedule);
            _dbContext.SaveChanges();
            Console.WriteLine("Next run time updated successfully.");
        }
    }
}
