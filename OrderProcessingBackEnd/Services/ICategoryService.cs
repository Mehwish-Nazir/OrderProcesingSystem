using OrderProcessingBackEnd.AutoMapper;
using OrderProcessingBackEnd.Data;
using OrderProcessingBackEnd.Repository;
using OrderProcessingBackEnd.DTO;
using OrderProcessingBackEnd.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Linq.Expressions;
namespace OrderProcessingBackEnd.Services
{
    public interface ICategoryService
    {
       Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> GetCategoryWithProductsAsync(int categoryId);
        Task<List<CategoryDTO>> GetAllCategoryAsync();
        Task<CategoryDTO> AddCategoryAsync(CategoryDTO categoryDTO);
        Task<CreateCategoryWithProductsDTO> AddCategoryWithProductAsync(CreateCategoryWithProductsDTO dto);
        Task<CategoryDTO> UpdateCategoryAsync(int id, CategoryDTO categoryDTO);
        Task<CategoryDTO> UpdateCategoryAlongProductAsync(int id, UpdateCategoryWithProductDTO categoryDTO);

    }
}
