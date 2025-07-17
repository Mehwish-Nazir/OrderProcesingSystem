using OrderProcessingBackEnd.Models;
using OrderProcessingBackEnd.Repository;
using OrderProcessingBackEnd.Services;
using OrderProcessingBackEnd.Data;
using OrderProcessingBackEnd.DTO;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using OrderProcessingBackEnd.AutoMapper;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
namespace OrderProcessingBackEnd.Services
{
    public class OrderService:IOrderService
    {
        private readonly IRepository<Orders> _orderRepository;
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;
        public OrderService(IRepository<Orders> orderRepository, IMapper mapper, ICustomerService customerService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _customerService = customerService;
        }

        public async Task<ActionResult> AddOrderDetail(OrdersDTO ordersDTO)
        {
            if (ordersDTO.CustomerID == null)
            {
                return new BadRequestObjectResult("CustomerID is required.");
            }

            var CustomerExist = await _customerService.getCustomerById(ordersDTO.CustomerID);
            if (CustomerExist == null)
            {
                return new NotFoundObjectResult($"Customer with ID {ordersDTO.CustomerID} not found");
            }
           
            //New Step: Validate Order Date (must be today or future)

            // 1. Define the time zone

            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
           
            // you can replace this if needed, depending on your system

            // 2. Convert the incoming OrderDate to UTC
            
           // DateTime utcOrderDate = TimeZoneInfo.ConvertTimeToUtc(ordersDTO.OrderDate, timeZone);

            // 3. Compare with current UTC Date
            var utcOrderDate = DateTime.SpecifyKind(ordersDTO.OrderDate, DateTimeKind.Utc);

            if (utcOrderDate.Date < DateTime.UtcNow.Date)
            {
                return new BadRequestObjectResult("Order date must be today or a future date. Past dates are not allowed.");
            }
            var name = CustomerExist.FirstName;

            var order = _mapper.Map<Orders>(ordersDTO);
            await _orderRepository.AddAsync(order);
            return new OkObjectResult($"Order details of {name} with customerId {ordersDTO.CustomerID} has added successfully");
        }
    }
}
