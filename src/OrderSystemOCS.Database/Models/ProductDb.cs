using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderSystemOCS.Database.Models
{
    [Table("products", Schema = "public")]
    public sealed class ProductDb
    {
        [Key]
        public Guid Id { get; set; }

        List<LineDb>? Lines { get; set; } = new();
    }
}
