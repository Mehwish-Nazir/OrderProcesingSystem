using OrderProcessingBackEnd.Models;
using OrderProcessingBackEnd.Repository;
using OrderProcessingBackEnd.Services;
using OrderProcessingBackEnd.AutoMapper;
using OrderProcessingBackEnd.Data;
using OrderProcessingBackEnd.DTO;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
namespace OrderProcessingBackEnd.Services
{
    public interface IOrderService
    {
        Task<ActionResult> AddOrderDetail(OrdersDTO ordersDTO);


    }
}
