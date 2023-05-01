using OrderSystemOCS.Domain;
using OrderSystemOCS.WebApi.Models;

namespace OrderSystemOCS.WebApi.Extensions
{
    public static class MapExtensions
    {
        public static IEnumerable<Line> MapToLine(this IEnumerable<LineDTO> lines)
        {
            foreach (LineDTO line in lines)
            {
                yield return new Line(line.Id, line.Qty);
            }
        }

        public static IEnumerable<LineDTO> MapToLineDTO(this IEnumerable<Line> lines)
        {
            foreach (Line line in lines)
            {
                yield return new LineDTO { Id = line.Product.Id, Qty = line.Qty };
            }
        }

        public static OrderResponse MapToOrderResponse(this Order order) =>
            new OrderResponse { Id = order.Id, Status = order.Status.ToString(), Created = order.Created, Lines = order.Lines.MapToLineDTO() };
        
        public static IEnumerable<OrderResponse> MapToOrderResponse(this IEnumerable<Order> orders) 
        {
            foreach (var order in orders)
            {
                yield return order.MapToOrderResponse();
            }
        }
    }
}
