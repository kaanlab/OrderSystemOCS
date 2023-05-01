namespace OrderSystemOCS.WebApi.Models
{
    public sealed class NewOrderRequest
    {
        public List<LineDTO> Lines { get; set; }
    }

}
