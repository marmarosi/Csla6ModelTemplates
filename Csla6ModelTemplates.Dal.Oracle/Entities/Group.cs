using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Csla6ModelTemplates.Dal.Oracle.Entities
{
    [Table("Groups")]
    public class Group : Timestamped
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? GroupKey { get; set; }

        [MaxLength(10)]
        public string GroupCode { get; set; }

        [MaxLength(100)]
        public string GroupName { get; set; }

        public ICollection<GroupPerson> Persons { get; set; }
    }
}
