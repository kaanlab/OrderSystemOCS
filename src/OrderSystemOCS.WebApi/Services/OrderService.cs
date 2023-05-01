using OrderSystemOCS.Domain;
using OrderSystemOCS.Domain.Interfaces;
using OrderSystemOCS.WebApi.Exceptions;
using OrderSystemOCS.WebApi.Extensions;
using OrderSystemOCS.WebApi.Models;

namespace OrderSystemOCS.WebApi.Services
{
    public sealed class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }
        public async Task<IReadOnlyList<OrderResponse>> GetAll()
        {
            var orders = await _repository.Load();
            return orders.MapToOrderResponse().ToList().AsReadOnly();
        }

        public async Task<OrderResponse> GetById(Guid id)
        {
            var order = await _repository.LoadById(id);
            return order.MapToOrderResponse();
        }

        public async Task<OrderResponse> Create(NewOrderRequest request) 
        {
            var lines = request.Lines.MapToLine();
            var order = new Order(lines);
            await _repository.Save(order);

            return order.MapToOrderResponse();
        }

        public async Task<OrderResponse> Update(Guid id, UpdateOrderRequest request)
        {
            var order = await _repository.LoadById(id);
            if(order is null)
            {
                throw new WebApiException("заказ не найден!");
            }

            if(order.Id != request.Id) 
            {
                throw new WebApiException("id заказа не совпадают!");
            }

            var lines = request.Lines.MapToLine().ToList();
            order.Update(request.Status, lines);
            await _repository.Save(order);

            return order.MapToOrderResponse();
        }

        public async Task<bool> Delete(Guid id) 
        {
            var order = await _repository.LoadById(id);
            if(order is null)
            {
                return false;
            }

            order.Delete();
            await _repository.Save(order);

            return true;
        }
    }
}
