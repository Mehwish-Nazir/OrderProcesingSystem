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
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace OrderProcessingBackEnd.Controllers
{
    [ApiController]
    [Route("api/categories")]  // Or any other path like api/category
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("allCategories")]
        public async Task<ActionResult<List<CategoryDTO>>> GetAllCategoryAsync()
        {
            var categories = await _categoryService.GetAllCategoryAsync();
            if (categories == null || !categories.Any())
            {
                return NotFound("No category found.");
            }
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);

                if (category == null)
                {
                    return NotFound(new { message = $"Category with ID {id} not found." });
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                //_logger.LogError(ex, "Error while retrieving category by ID.");

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An unexpected error occurred while fetching the category.",
                    ex=ex.InnerException
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add_category")]

        public async Task<ActionResult<CategoryDTO>> AddCategory([FromBody] CategoryDTO categoryDTO)
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
                    message = "Only Admins are allowed to add Categories of Items"
                });
            }

            try
            {
                var addCategory = await _categoryService.AddCategoryAsync(categoryDTO);
                return CreatedAtAction(nameof(GetCategoryById), new { id = addCategory.CategoryID }, new
                {
                    message = $"{addCategory.CategoryName} with id '{addCategory.CategoryID}'. ",
                    data = addCategory
                });
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
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An unexpected error occurred.",
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }


        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add-category-with-product")]
        public async Task<ActionResult<CreateCategoryWithProductsDTO>> CreateCategoryWithProduct([FromBody] CreateCategoryWithProductsDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _categoryService.AddCategoryWithProductAsync(dto);
                return CreatedAtAction(nameof(GetCategoryById), new { name = result.CategoryName }, new
                {
                    message = $"Category '{result.CategoryName}' created.",
                    data = result
                });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message,
                    deepestInner = ex.InnerException?.InnerException?.Message
                });
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

