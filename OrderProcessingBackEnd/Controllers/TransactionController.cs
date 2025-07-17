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

namespace OrderProcessingBackEnd.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
       
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("PaymentMethod")]
        public IActionResult GetPaymentMethod()
        {
            var paymentMethod = Enum.GetNames(typeof(PaymentMethod)).ToList();
            return Ok(paymentMethod);
        }

        [HttpGet("TransactionStatus")]
        public IActionResult GetTransactionStatus()
        {
            var status = Enum.GetNames(typeof(TransactionStatus)).ToList();
            return Ok(status);
        }


    }

}
