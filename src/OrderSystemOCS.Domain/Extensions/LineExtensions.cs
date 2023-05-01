namespace OrderSystemOCS.Domain.Extensions
{
    static class LineExtensions
    {
        public static IEnumerable<Line> SumQtyByProduct(this IEnumerable<Line> sequense) =>
            sequense.GroupBy(p => p.Product).Select(x => new Line(x.Key.Id, x.Sum(y => y.Qty)));  
    }
}
