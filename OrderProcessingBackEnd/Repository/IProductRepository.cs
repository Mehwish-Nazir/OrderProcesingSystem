using OrderProcessingBackEnd.DTO;
using OrderProcessingBackEnd.AutoMapper;
namespace OrderProcessingBackEnd.Repository
{
    public interface IProductRepository
    
       
        {
            Task<PagedProductResponseDto> SearchProductsAsync(ProductSearchRequestDto request);
        }
    
}
