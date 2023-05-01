using FluentAssertions;

namespace OrderSystemOCS.Domain.Tests
{
    public class LineTests
    {
        [Fact(DisplayName = "Negative amount of \"Qty\" should return exception")]
        public void Test_01()
        {
            Action action = () => new Line(Guid.NewGuid(), -1);

            action.Should().Throw<ArgumentOutOfRangeException>().WithMessage("количество по строке заказа не может быть отрицательным (Parameter 'qty')");

        }

        [Fact(DisplayName = "Zero amount of \"Qun\" should return exception")]
        public void Test_02()
        {
            Action action = () => new Line(Guid.NewGuid(), 0);

            action.Should().Throw<ArgumentOutOfRangeException>().WithMessage("количество по строке заказа не может быть 0 (Parameter 'qty')");

        }

        [Fact(DisplayName = "Two lines should be equals if these fields are equals")]
        public void Test_03()
        {
            var product = Product.Create();
            var line1 = new Line(product.Id, 5);
            var line2 = new Line(product.Id, 5);

            line1.Should().Be(line2);
        }
    }
}
