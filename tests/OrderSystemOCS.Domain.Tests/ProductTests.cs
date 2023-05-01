using FluentAssertions;

namespace OrderSystemOCS.Domain.Tests
{
    public class ProductTests
    {
        [Fact(DisplayName = "Two products should be equals if these fields are equals")]
        public void Test_01()
        {
            var guid = Guid.NewGuid();
            var product1 = new Product(guid);
            var product2 = new Product(guid);

            product1.Should().Be(product2);
        }
    }
}
