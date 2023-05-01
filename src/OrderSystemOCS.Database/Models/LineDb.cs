using System.ComponentModel.DataAnnotations.Schema;

namespace OrderSystemOCS.Database.Models
{
    [Table("lines", Schema = "public")]
    public sealed class LineDb
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Qty { get; set; }
        public ProductDb Product { get; set; }
        public OrderDb? Order { get; set; }
    }
}
