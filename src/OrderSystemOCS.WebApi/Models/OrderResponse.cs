namespace OrderSystemOCS.WebApi.Models
{
    public sealed class OrderResponse
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
        public IEnumerable<LineDTO> Lines { get; set; }
    }
}
