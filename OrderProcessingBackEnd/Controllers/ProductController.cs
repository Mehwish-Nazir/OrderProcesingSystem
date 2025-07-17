using OrderProcessingBackEnd.AutoMapper;
using OrderProcessingBackEnd.Data;
using OrderProcessingBackEnd.Repository;
using OrderProcessingBackEnd.DTO;
using OrderProcessingBackEnd.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using  OrderProcessingBackEnd.Services;
namespace OrderProcessingBackEnd.Controllers
{
    [ApiController]
    [Route("api/product")]  // Or any other path like api/category
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            if (id < 0)
            {
                return BadRequest(new { message = "ID must not be negative." });
            }

            try
            {
                var product = await _productService.GetProductByIdAsync(id);

                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ("An unexpected error occurred. Please try again later.", ex.Message),

                    innerException = ex.InnerException
                });

            }
        }


        [HttpGet("fecthProductWithCategory")]
        public async Task<ActionResult<List<ProductWithCategoryDTO>>> GetProductWithCategoryAsync()
        {
            try
            {
                var products = await _productService.GetProductsWithCategory();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);

            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("add-product")]

        public async Task<ActionResult<CreateNewProductDTO>> AddNewProductAsync([FromBody] CreateNewProductDTO productDTO)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var addProduct = await _productService.AddProductAsync(productDTO);
                return CreatedAtAction(nameof(GetProductById), new { id = addProduct.ProductID }, new
                {
                    message = $"The product {productDTO.ProductName} with id {addProduct.ProductID} has been successfully added.",
                    data = addProduct
                });

            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { message = ex.Message });

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

        [HttpGet("getProductbyCategory/{categoryId}")]
        public async Task<ActionResult<List<ProductDTO>>> GetProductListByCategory(int categoryId)
        {
            var products = await _productService.GetProductByCategory(categoryId);

            if (products == null || !products.Any())
            {
                return NotFound($"No products found for category ID {categoryId}");
            }

            return Ok(products);
        }


        [Authorize]
        [HttpPost("search")]
        public async Task<IActionResult> SearchProducts([FromBody] ProductSearchRequestDto request)
        {
            try
            {
                var result = await _productService.SearchProductsAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Bad request: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Application error: {Message}", ex.Message);
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred.");
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }

        }
    }
}
