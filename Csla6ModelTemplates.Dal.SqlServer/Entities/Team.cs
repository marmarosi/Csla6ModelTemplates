using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Csla6ModelTemplates.Dal.SqlServer.Entities
{
    [Table("Teams")]
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? TeamKey { get; set; }

        [MaxLength(10)]
        public string TeamCode { get; set; }

        [MaxLength(100)]
        public string TeamName { get; set; }

        public DateTime Timestamp { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}
