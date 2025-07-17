
using Microsoft.EntityFrameworkCore;
using OrderProcessingBackEnd.Data;
using OrderProcessingBackEnd.DTO;

namespace OrderProcessingBackEnd.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly OrderProcessingDbContext _context;

        public ProductRepository(OrderProcessingDbContext context)
        {
            _context = context;
        }

        public async Task<PagedProductResponseDto> SearchProductsAsync(ProductSearchRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText))
                throw new ArgumentException("Search text is required");

            var text = request.SearchText.Trim().ToLower();

            var query = _context.Product
                .Include(p => p.Category)
                .Include(p => p.OrderProducts)
                    .ThenInclude(op => op.Order)
                        .ThenInclude(o => o.Customer)
                            .ThenInclude(c => c.User)
                .Where(p => p.Category.CategoryName.ToLower().Contains(text)
                         || p.ProductName.ToLower().Contains(text))
                .AsQueryable();

            int totalCount = await query.CountAsync();

            var products = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new ProductResponseDto
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Stock = p.Stock,
                    CreatedAt = p.CreatedAt,
                    CategoryName = p.Category.CategoryName,
                    StockStatus = p.Stock == 0 ? "Out of Stock" : "In Stock"

                })
                .ToListAsync();

            return new PagedProductResponseDto
            {
                Products = products,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
