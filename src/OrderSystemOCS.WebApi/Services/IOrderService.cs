using OrderSystemOCS.WebApi.Models;

namespace OrderSystemOCS.WebApi.Services
{
    public interface IOrderService
    {
        Task<IReadOnlyList<OrderResponse>> GetAll();
        Task<OrderResponse> GetById(Guid id);
        Task<OrderResponse> Create(NewOrderRequest request);
        Task<OrderResponse> Update(Guid id, UpdateOrderRequest request);
        Task<bool> Delete(Guid id);
    }
}
