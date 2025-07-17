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

namespace OrderProcessingBackEnd.DTO
{
    public class PlaceOrderItemDTO
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; } // calculated inside backend (not in request body)
        //public int? Stock { get; set; } // optional: backend use only, not in request
    }

    public class PlaceOrderDTO
    {
        public DateTime OrderDate { get; set; }
        public List<PlaceOrderItemDTO> Items { get; set; }
        public string PaymentMethod { get; set; }
    }

    //get the order detail DTO
    public class GetOrderDetailsDTO
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public List<GetOrderItemDTO> Items { get; set; }
      //  public int ProductID { get; set; }
    }

    public class GetOrderItemDTO
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }

    }


}
