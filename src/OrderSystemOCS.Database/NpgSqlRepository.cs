using Microsoft.EntityFrameworkCore;
using OrderSystemOCS.Database.Mappings;
using OrderSystemOCS.Database.Models;
using OrderSystemOCS.Domain;
using OrderSystemOCS.Domain.Interfaces;

namespace OrderSystemOCS.Database
{
    internal sealed class NpgSqlRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public NpgSqlRepository(AppDbContext context) => _context = context;        

        public async Task<Order> LoadById(Guid orderId)
        {
            var dbOrder = await _context.Orders
                .AsNoTracking()
                .Include(l => l.Lines)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.IsDeleted == false);

            return App.Mapper.Map<Order>(dbOrder);
        }

        public async Task<IReadOnlyList<Order>> Load()
        {
            var dbOrders = await _context.Orders
                .AsNoTracking()
                .Where(o => o.IsDeleted == false)
                .Include(l => l.Lines)
                .ThenInclude(p => p.Product)
                .ToListAsync();

            return App.Mapper.Map<IReadOnlyList<Order>>(dbOrders);
        }

        public async Task Save(Order order)
        {
            var dbOrder = await _context.Orders
                .Include(l => l.Lines)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(o => o.Id.Equals(order.Id) && o.IsDeleted == false);

            if (dbOrder is not null)
            {
                // update
                dbOrder.Status = order.Status;
                dbOrder.IsDeleted = order.IsDeleted;
                dbOrder.DeletedAt = order.DeletedAt;
                dbOrder.Lines.Clear();
                dbOrder.Lines = await GetNewLines(order.Lines);
            }
            else
            {
                // new
                var newDbOrder = App.Mapper.Map<OrderDb>(order);
                newDbOrder.Lines = await GetNewLines(order.Lines);
                await _context.Orders.AddAsync(newDbOrder);
            }

            await _context.SaveChangesAsync();
        }

        private async Task<List<LineDb>> GetNewLines(IEnumerable<Line> lines)
        {
            List<LineDb> newLines = new List<LineDb>();
            foreach (var line in lines)
            {
                var productId = line.Product.Id;
                var productDb = await _context.Products.FirstOrDefaultAsync(p => p.Id.Equals(productId));
                if (productDb is null)
                {
                    productDb = new ProductDb { Id = line.Product.Id };
                    await _context.Products.AddAsync(productDb);
                    await _context.SaveChangesAsync();
                }

                var lineDb = new LineDb { Product = productDb, Qty =  line.Qty };
                await _context.Lines.AddAsync(lineDb);
                await _context.SaveChangesAsync();

                newLines.Add(lineDb);
            }

            return newLines;
        }
    }
}
