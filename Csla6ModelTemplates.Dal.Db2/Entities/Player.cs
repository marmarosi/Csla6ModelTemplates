using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Csla6ModelTemplates.Dal.Db2.Entities
{
    [Table("Players")]
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? PlayerKey { get; set; }

        public long? TeamKey { get; set; }

        [MaxLength(10)]
        public string PlayerCode { get; set; }

        [MaxLength(100)]
        public string PlayerName { get; set; }

        [ForeignKey("TeamKey")]
        public Team Team { get; set; }
    }
}
