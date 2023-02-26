using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Csla6ModelTemplates.Dal.SqlServer.Entities
{
    [Table("Folders")]
    public class Folder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? FolderKey { get; set; }

        public long? ParentKey { get; set; }

        public long? RootKey { get; set; }

        public int? FolderOrder { get; set; }

        [MaxLength(100)]
        public string FolderName { get; set; }

        public DateTime Timestamp { get; set; }

        [ForeignKey("ParentKey")]
        public Folder Parent { get; set; }

        public ICollection<Folder> Children { get; set; }
    }
}
