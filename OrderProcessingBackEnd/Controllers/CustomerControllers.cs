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
    [Route("api/customers")]  // Or any other path like api/category
    public class CustomerControllers:ControllerBase
    {

        private readonly ICustomerService _customerService;
        public CustomerControllers(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var customersList = await _customerService.GetCustomersAsync();
            if (customersList == null || !customersList.Any())
            {
                return NotFound("No customers with user data found.");
            }
            return Ok(customersList);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
        {
            try
            {
                var customer = await _customerService.getCustomerById(id);

                if (customer == null)
                {
                    return NotFound(new { message = $"Customer with ID {id} not found." });
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                //_logger.LogError(ex, "Error while retrieving customer by ID.");

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message =( "An unexpected error occurred while fetching the customer.",ex.Message)
                });
            }
        }

        [HttpGet("with-users")]
        public async Task<IActionResult> GetCustomersWithUsers()
        {
            var customers = await _customerService.GetCustomersWithUsersAsync();

            var validCustomers = customers
                .Where(c => c.User != null && c.UserID ==c.User.UserID)
                .ToList();

            if (!validCustomers.Any())
                return NotFound($"No customers found for UserId.");

            return Ok(validCustomers);

        }
        [Authorize(Roles="Customer")]
        [HttpPost("AddCustomers")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
        // In your controller action, you can check
        // ModelState.IsValid to ensure the incoming model adheres to the defined annotations lie [Reuqired] [MaxLenght].
        //
        public async Task<ActionResult<CustomerDto>> AddCustomers(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)  //it will ensure annotation like [Required][Max Length] are fulfilled 
            {
                return BadRequest(ModelState);
            }


            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Customer")
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    message = " Only Customer  users are allowed to fill this form."
                });
            }
            try
            {
                
                var addCustomer = await _customerService.AddCustomers(customerDto);
                
                    return CreatedAtAction(nameof(GetCustomerById), new { id = addCustomer.CustomerID }, new
                    {
                        message = $" {addCustomer.FirstName} {addCustomer.LastName}, your record has been successfully added.",
                        data = addCustomer
                    });
                
               

                /* //return CreatedAtAction(
                 //nameof(GetCustomerById), // Name of the GET action
                 //new { id = addedCustomer.CustomerID }, // Route values
                 //addedCustomer );           // Response body   */
                // return Created(string.Empty, addCustomer); // Temporarily return Created without location

            }
            catch (ArgumentNullException ex)
            {
                // Handle specific exception when customer data is null
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Handle specific exception when customer already exists
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception details here (e.g., using a logging framework)
                // For example: _logger.LogError(ex, "An unexpected error occurred.");
                // Return a generic 500 Internal Server Error response

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred." ,
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }


    }


}
