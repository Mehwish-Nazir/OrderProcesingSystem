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
namespace OrderProcessingBackEnd.Services

{
    public class OrderPlaceService : IOrderPlaceService
    {
        private readonly OrderProcessingDbContext _dbContext;
        public OrderPlaceService(OrderProcessingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<string> OrderPlaceByCustomerAsync(int authenticatedUserId, PlaceOrderDTO dto)
{
    using var transaction = await _dbContext.Database.BeginTransactionAsync();

    try
    {
        var customer = await _dbContext.Customers
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.UserID == authenticatedUserId);

                if (customer == null)
                {
                     //var username = User?.Identity?.Name ?? "Unknown";
                    var username = customer?.User?.Username;
                    return $"Customer not found with user {username}.";
                }


                var order = new Orders
                {
                    OrderDate = dto.OrderDate,
                    CustomerID = customer.CustomerID,
                    OrderStatus = orderStatus.Pending.ToString(),
                    OrderProducts = new List<OrderProducts>(),

                };

        decimal totalAmount = 0;

        foreach (var item in dto.Items)
        {
            var product = await _dbContext.Product.FirstOrDefaultAsync(p => p.ProductName == item.ProductName);
             decimal backendPrice = product.Price;

                    if (product == null)
            {
                await transaction.RollbackAsync();
                return $"Product '{item.ProductName}' not found.";
            }

            if (item.Quantity > product.Stock)
            {
                await transaction.RollbackAsync();
                return $"Not enough stock for {product.ProductName}.";
            }
                  //  var product = await _dbContext.Product.FirstOrDefaultAsync(p => p.Name == itemDTO.ProductName);
                    product.Stock -= item.Quantity;

            var orderProduct = new OrderProducts
            {
                ProductID = product.ProductID,
                Quantity = item.Quantity,
                PriceAtPurchase = backendPrice

            };

            totalAmount += orderProduct.PriceAtPurchase * orderProduct.Quantity;
            order.OrderProducts.Add(orderProduct);
        }

        order.TotalAmount = totalAmount;

        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();

        if (!Enum.TryParse<PaymentMethod>(dto.PaymentMethod, out var parsedMethod))
        {
            await transaction.RollbackAsync();
            return "Invalid payment method.";
        }

        var transactionEntity = new Transactions
        {
            PaymentMethod = parsedMethod.ToString(),
            AmountPaid = order.TotalAmount,
            TransactionDate = DateTime.UtcNow,
            TransactionStatus = TransactionStatus.Pending.ToString(),
            OrderID = order.OrderID
        };

        await _dbContext.Transactions.AddAsync(transactionEntity);
        await _dbContext.SaveChangesAsync();

        await transaction.CommitAsync();

        return $"{customer.FirstName} ({customer.User.Username}) with user id {customer.User.UserID}, your order has been successfully placed!";
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        // Log exception if you have logging infrastructure
        return "An unexpected error occurred while placing your order.";
    }
}


        public async Task<GetOrderDetailsDTO> GetOrderDetailsAsync(int orderId, int userId, string role)
        {
            var order = await _dbContext.Orders
       .Include(o => o.Customer) // Include customer details
       .Include(o => o.OrderProducts) // Include products in the order
           .ThenInclude(op => op.Product) // Include product details (name, price, etc.)
       .Include(o => o.Transaction) // Include transaction details (payment method, etc.)
       .FirstOrDefaultAsync(o => o.OrderID == orderId); // Fetch order by ID

            if (order == null)
            {
                return null;
            }

            if (role == "Customer" && order.Customer.UserID != userId)
            {
                return null; ///frobid excepton
            }
            var transaction = order.Transaction.FirstOrDefault();

            var result = new GetOrderDetailsDTO
            {
                OrderID = order.OrderID,
                CustomerName = $"{order.Customer.FirstName} {order.Customer.LastName}", // Full customer name
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                TotalAmount = order.TotalAmount,
                PaymentMethod = transaction?.PaymentMethod ?? "N/A",  // If no transaction, set to "N/A"
                Items = order.OrderProducts.Select(op => new GetOrderItemDTO
                {
                    ProductName = op.Product.ProductName,  // Product name
                    Quantity = op.Quantity,  // Quantity purchased
                    PriceAtPurchase = op.PriceAtPurchase  // Price at the time of purchase
                }).ToList()  // Convert the order products into DTOs for each item
            };

            return result;
        }



    }





}
