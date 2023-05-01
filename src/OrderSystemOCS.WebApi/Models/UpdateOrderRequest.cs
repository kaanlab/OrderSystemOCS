using OrderSystemOCS.Domain;

namespace OrderSystemOCS.WebApi.Models
{
    public class UpdateOrderRequest
    {
        public Guid Id { get; set; }
        public Status Status { get; set; }
        public List<LineDTO> Lines { get; set; }

    }
}
