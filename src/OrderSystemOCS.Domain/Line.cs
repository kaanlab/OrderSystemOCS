namespace OrderSystemOCS.Domain
{
    public sealed class Line : ValueObject
    {
        public Product Product { get; private set; }
        public int Qty { get; private set; }

        private Line()
        {
                
        }

        public Line(Guid productId, int qty)
        {
            if (qty < 0) throw new ArgumentOutOfRangeException(nameof(qty), "количество по строке заказа не может быть отрицательным");
            if (qty == 0) throw new ArgumentOutOfRangeException(nameof(qty), "количество по строке заказа не может быть 0");

            Product = new Product(productId);
            Qty = qty;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Product.Id;
            yield return Qty;
        }
    }
}
