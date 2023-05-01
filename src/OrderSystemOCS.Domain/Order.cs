using OrderSystemOCS.Domain.Exceptions;
using OrderSystemOCS.Domain.Extensions;

namespace OrderSystemOCS.Domain
{
    public sealed class Order
    {
        public Guid Id { get; private set; }
        public Status Status { get; private set; }
        public DateTime Created { get; private set; }        
        public IReadOnlyList<Line> Lines { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTimeOffset? DeletedAt { get; private set; }

        private Order()
        {
                
        }

        public Order(IEnumerable<Line> lines)
        {
            if (lines.Count() == 0) throw new ArgumentOutOfRangeException(nameof(lines), "невозможно создать заказ без строк");

            Id = Guid.NewGuid();
            Status = Status.New;
            Created = DateTime.UtcNow;            
            Lines = lines.SumQtyByProduct().ToList();
        }

        public void Update(Status status, IEnumerable<Line> lines)
        {
            if (lines.Count() == 0) throw new ArgumentOutOfRangeException(nameof(lines), "невозможно обновить заказ без строк");

            if ((Status == Status.Paid 
                || Status == Status.ToDelivery 
                || Status == Status.Delivered 
                || Status == Status.Completed) && !Lines.SequenceEqual(lines))
            {
                throw new ArgumentException("заказы в статусах \"оплачен\", \"передан в доставку\", \"доставлен\", \"завершен\" нельзя редактировать", nameof(status));
            }

            Lines = lines.SumQtyByProduct().ToList();
            Status = status;            
        }

        public void Delete()
        {
            if (IsDeleted) throw new DomainException("невозможно удалить заказ дважды");

            if (Status == Status.ToDelivery
                || Status == Status.Delivered
                || Status == Status.Completed)
            {
                throw new DomainException("заказы в статусах “передан в доставку”, “доставлен”, “завершен” нельзя удалить");
            }

            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }
    }
}
