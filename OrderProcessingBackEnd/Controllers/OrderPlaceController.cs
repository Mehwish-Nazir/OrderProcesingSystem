using OrderProcessingBackEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using OrderProcessingBackEnd.Controllers;
using OrderProcessingBackEnd.DTO;
using OrderProcessingBackEnd.Models;
using OrderProcessingBackEnd.Repository;
using OrderProcessingBackEnd.Services;
using Route = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Serilog;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using OrderProcessingBackEnd.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace OrderProcessingBackEnd.Controllers
{
    [ApiController]
    [Route("api/OrderPlace")]
    public class OrderPlaceController : ControllerBase
    {
        private readonly IOrderPlaceService _orderPlaceService;
        private readonly OrderProcessingDbContext _context;
        public OrderPlaceController(IOrderPlaceService orderPlaceService, OrderProcessingDbContext context)
        {
            _orderPlaceService = orderPlaceService;
            _context = context;
        }

        [Authorize]
        [HttpPost("placeOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Invalid user ID.");

            var result = await _orderPlaceService.OrderPlaceByCustomerAsync(userId, dto);

            if (result.Contains("not found") || result.Contains("not enough stock") || result.Contains("invalid"))
            {
                return BadRequest(new { message = result });
            }

            if (result.Contains("unexpected error"))
            {
                return StatusCode(500, new { message = result });
            }

            return Ok(new { message = result });
        }


        [Authorize]
        [HttpGet("orderDetails/{orderId}")]
        public async Task<ActionResult> GetOrderDetailById([FromRoute] int orderId)
        {
            bool OrderExist = _context.Orders.Any(o => o.OrderID == orderId);

            if (!OrderExist)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            // Retrieve the user ID and role from the JWT token claims
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Customer";

            // Fetch order details from the service layer
            var orderDetails = await _orderPlaceService.GetOrderDetailsAsync(orderId, userId, role);

            if (orderDetails == null)
            {
                return NotFound("Order not found or access denied"); // Return 404 if order details are not found or unauthorized
            }

            return Ok(orderDetails); // Return the order details as a successful response
        }

    }
}
