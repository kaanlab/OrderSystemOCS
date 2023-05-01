namespace OrderSystemOCS.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task Save(Order order);
        Task<Order> LoadById(Guid orderId);
        Task<IReadOnlyList<Order>> Load();
    }
}
