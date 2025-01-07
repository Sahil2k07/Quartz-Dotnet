using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quartz.entity
{
    [Table("QUARTZ_SCHEDULE")]
    public class QuartzSchedule
    {
        [Key]
        public int Id { get; set; }
        public required string JobName { get; set; }
        public DateTime NextRunTime { get; set; }
    }
}
