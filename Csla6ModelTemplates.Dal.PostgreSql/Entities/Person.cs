using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Csla6ModelTemplates.Dal.PostgreSql.Entities
{
    [Table("Persons")]
    public class Person : Timestamped
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? PersonKey { get; set; }

        [MaxLength(10)]
        public string PersonCode { get; set; }

        [MaxLength(100)]
        public string PersonName { get; set; }

        public ICollection<GroupPerson> Groups { get; set; }
    }
}
