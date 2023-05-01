namespace OrderSystemOCS.Domain
{
    public sealed class Product : ValueObject
    {
        public Guid Id { get; private set; }

        private Product()
        {
                
        }

        public static Product Create()
        {
            return new Product(Guid.NewGuid());
        }

        public Product(Guid id) => Id = id;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
