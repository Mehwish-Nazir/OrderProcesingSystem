using Microsoft.AspNetCore.Mvc;
using OrderProcessingBackEnd.Models;
using OrderProcessingBackEnd.Controllers;
using OrderProcessingBackEnd.Data;
using OrderProcessingBackEnd.Repository;
using OrderProcessingBackEnd.DTO;
using OrderProcessingBackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
namespace OrderProcessingBackEnd.Controllers
{
    [ApiController]
    [Route("api/order")]  // Or any other path like api/category
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        
        [HttpGet("OrderStatus")]
        public IActionResult getOrderStatus()
        {
            var orderStatus = Enum.GetNames(typeof(OrderStatus)).ToList();
            return Ok(orderStatus);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("add-order-detail")]
        public async Task<ActionResult<OrdersDTO>> AddOrderDetails([FromBody] OrdersDTO ordersDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
             var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    message = " Only Admin users are allowed to add order Details."
                });
            }
                try
                {
                var order = await _orderService.AddOrderDetail(ordersDTO);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An unexpected error occurred.",
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }
        }

    }


