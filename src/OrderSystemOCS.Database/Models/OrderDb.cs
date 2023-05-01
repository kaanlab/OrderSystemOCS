using OrderSystemOCS.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderSystemOCS.Database.Models
{
    [Table("orders", Schema = "public")]
    public sealed class OrderDb
    {
        [Key]
        public Guid Id { get; set; }
        public Status Status { get; set; }
        public DateTime Created { get; set; }
        public List<LineDb> Lines { get; set; } = new();
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
