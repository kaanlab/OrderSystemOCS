using FluentAssertions;
using OrderSystemOCS.Domain.Exceptions;

namespace OrderSystemOCS.Domain.Tests
{
    public class OrderTests
    {
        [Fact(DisplayName = "New Order should return status \"New\" and creation data in UTC format")]
        public void Test_01()
        {
            var products = new List<Product>
            {
                Product.Create(),
                Product.Create()
            };

            var lines = new List<Line>
            {
                new Line(products[0].Id, 3),
                new Line(products[1].Id, 1)
            };

            var order = new Order(lines);

            order.Id.Should().NotBeEmpty();
            order.Created.Should().BeIn(DateTimeKind.Utc);
            order.Status.Should().Be(Status.New);
            order.Lines.Should().HaveCount(2);            
        }

        [Fact(DisplayName = "Order with empty lines should return exception")]
        public void Test_02()
        {
            var lines = new List<Line>();
            Action action = () => new Order(lines);

            action.Should().Throw<ArgumentOutOfRangeException>().WithMessage("невозможно создать заказ без строк (Parameter 'lines')");
        }

        [Fact(DisplayName = "Order with null lines should return exception")]
        public void Test_03()
        {
            List<Line> lines = null;
            Action action = () => new Order(lines);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory(DisplayName = "Orders in statuses \"New\" and \"Awaiting\" should allow to editing lines")]
        [MemberData(nameof(OrdersWithStatusNewAndAwaiting))]
        public void Test_04(Order order)
        {
            var orderId = order.Id;
            var orderCreatedTime = order.Created;
            var updateLines = new List<Line>
            {
                new Line(Guid.NewGuid(), 5)
            };

            order.Update(Status.AwaitingPayment, updateLines);

            order.Id.Should().Be(orderId);
            order.Created.Should().BeIn(DateTimeKind.Utc);
            order.Created.Should().Be(orderCreatedTime);
            order.Status.Should().Be(Status.AwaitingPayment);
            order.Lines.Should().HaveCount(1);
        }

        [Theory(DisplayName = "Orders in statuses \"Paid\", \"ToDelivery\" \"Delivered\" and \"Completed\" should not allow to editing lines")]
        [MemberData(nameof(OrdersWithStatusPaidToDeliveryDeliveredCompleted))]
        public void Test_05(Order order)
        {
            Action action = () =>
            {
                var updateLines = new List<Line>
                {
                    new Line(Guid.NewGuid(), 5)
                };

                order.Update(Status.Completed, updateLines);
            };

            action.Should().Throw<ArgumentException>().WithMessage("заказы в статусах \"оплачен\", \"передан в доставку\", \"доставлен\", \"завершен\" нельзя редактировать (Parameter 'status')");
        }

        [Theory(DisplayName = "Orders in any statuses should allowed to change status")]
        [MemberData(nameof(OrdersWithStatusPaidToDeliveryDeliveredCompleted))]
        public void Test_06(Order order)
        {
            var productIds = order.Lines.Select(x => x.Product.Id).ToArray();
            var lines = new List<Line>()
            {
                new Line(productIds[0], 3),
                new Line(productIds[1], 1)
            };

            order.Update(Status.New, lines);

            order.Status.Should().Be(Status.New);
        }

        [Theory(DisplayName = "Orders in statuses \"New\" and \"Awaiting\" should return exception if it update with empty lines")]
        [MemberData(nameof(OrdersWithStatusNewAndAwaiting))]
        public void Test_07(Order order)
        {
            Action action = () =>
            {
                var updateLines = new List<Line>();

                order.Update(Status.Paid, updateLines);
            };

            action.Should().Throw<ArgumentOutOfRangeException>().WithMessage("невозможно обновить заказ без строк (Parameter 'lines')");
        }

        [Theory(DisplayName = "Orders in statuses \"New\" and \"Awaiting\" should be deleted")]
        [MemberData(nameof(OrdersWithStatusNewAndAwaiting))]
        public void Test_08(Order order)
        { 
            order.Delete();

            order.IsDeleted.Should().BeTrue();
        }

        [Theory(DisplayName = "Orders in statuses \"New\" and \"Awaiting\" should return exception if try to deleted twice")]
        [MemberData(nameof(OrdersWithStatusNewAndAwaiting))]
        public void Test_09(Order order)
        {
            Action action = () =>
            {
                order.Delete();
                order.Delete();
            };

            action.Should().Throw<DomainException>().WithMessage("невозможно удалить заказ дважды");
        }

        [Theory(DisplayName = "Orders in statuses \"ToDelivery\" \"Delivered\" and \"Completed\" should return exception when try to delete")]
        [MemberData(nameof(OrdersWithStatusToDeliveryDeliveredCompleted))]
        public void Test_10(Order order)
        {
            Action action = () => order.Delete();           

            action.Should().Throw<DomainException>().WithMessage("заказы в статусах “передан в доставку”, “доставлен”, “завершен” нельзя удалить");
        }

        [Fact(DisplayName = "Lines with the same product should sum its qty")]
        public void Test_11()
        {
            var guid = Guid.NewGuid();
            var products = new List<Product>
            {
                Product.Create(),
                Product.Create(),
                new Product(guid),
                new Product(guid)
            };

            var lines = new List<Line>
            {
                new Line(products[0].Id, 3),
                new Line(products[1].Id, 1),
                new Line(products[2].Id, 5),
                new Line(products[3].Id, 11)
            };

            var order = new Order(lines);
            var line = order.Lines.FirstOrDefault(l => l.Product.Id == guid);

            order.Id.Should().NotBeEmpty();
            order.Created.Should().BeIn(DateTimeKind.Utc);
            order.Status.Should().Be(Status.New);
            order.Lines.Should().HaveCount(3);
            line.Qty.Should().Be(16);
        }

        public static IEnumerable<object[]> OrdersWithStatusNewAndAwaiting => CreateOrdersWithStatusNewAndAwaiting();

        private static IEnumerable<object[]> CreateOrdersWithStatusNewAndAwaiting()
        {
            var products = new List<Product>
            {
                Product.Create(),
                Product.Create()
            };

            var lines = new List<Line>
            {
                new Line(products[0].Id, 3),
                new Line(products[1].Id, 1)
            };

            var firstOrder = new Order(lines);
            firstOrder.Update(Status.AwaitingPayment, lines);

            var secondOrder = new Order(lines);

            return new List<object[]> { 
                new object[] { firstOrder }, 
                new object[] { secondOrder } 
            };
        }

        public static IEnumerable<object[]> OrdersWithStatusPaidToDeliveryDeliveredCompleted => CreateOrdersWithStatusPaidToDeliveryDeliveredCompleted();

        private static IEnumerable<object[]> CreateOrdersWithStatusPaidToDeliveryDeliveredCompleted()
        {
            var products = new List<Product>
            {
                Product.Create(),
                Product.Create()
            };

            var lines = new List<Line>
            {
                new Line(products[0].Id, 3),
                new Line(products[1].Id, 1)
            };

            var firstOrder = new Order(lines);
            firstOrder.Update(Status.Paid, lines);

            var secondOrder = new Order(lines);
            secondOrder.Update(Status.ToDelivery, lines);

            var thirdOrder = new Order(lines);
            thirdOrder.Update(Status.Delivered, lines);

            var fourthOrder = new Order(lines);
            fourthOrder.Update(Status.Completed, lines);

            return new List<object[]> {
                new object[] { firstOrder },
                new object[] { secondOrder },
                new object[] { thirdOrder },
                new object[] { fourthOrder }
            };
        }

        public static IEnumerable<object[]> OrdersWithStatusToDeliveryDeliveredCompleted => CreateOrdersWithStatusToDeliveryDeliveredCompleted();

        private static IEnumerable<object[]> CreateOrdersWithStatusToDeliveryDeliveredCompleted()
        {
            var products = new List<Product>
            {
                Product.Create(),
                Product.Create()
            };

            var lines = new List<Line>
            {
                new Line(products[0].Id, 3),
                new Line(products[1].Id, 1)
            };

            var secondOrder = new Order(lines);
            secondOrder.Update(Status.ToDelivery, lines);

            var thirdOrder = new Order(lines);
            thirdOrder.Update(Status.Delivered, lines);

            var fourthOrder = new Order(lines);
            fourthOrder.Update(Status.Completed, lines);

            return new List<object[]> {
                new object[] { secondOrder },
                new object[] { thirdOrder },
                new object[] { fourthOrder }
            };
        }
    }
}
