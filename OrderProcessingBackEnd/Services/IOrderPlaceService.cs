using OrderProcessingBackEnd.DTO;

namespace OrderProcessingBackEnd.Services
{
    public interface IOrderPlaceService
    {
        Task<GetOrderDetailsDTO> GetOrderDetailsAsync(int orderId, int userId, string role);
        Task<string> OrderPlaceByCustomerAsync(int authenticatedUserId, PlaceOrderDTO dTO);

    }
}
