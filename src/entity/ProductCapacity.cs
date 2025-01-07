using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quartz.entity
{
    [Table("PRODUCTION_CAPACITY")]
    public class ProductionCapacity
    {
        [Key]
        public int ID { get; set; }
        public required string Identifier { get; set; }
        public DateTime Day { get; set; }
    }
}
