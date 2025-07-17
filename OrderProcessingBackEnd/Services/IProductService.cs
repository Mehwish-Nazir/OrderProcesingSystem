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

namespace OrderProcessingBackEnd.Services
{
    public interface IProductService
    {
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<List<ProductWithCategoryDTO>> GetProductsWithCategory();  
       Task<CreateNewProductDTO> AddProductAsync(CreateNewProductDTO productDTO);

        Task<List<ProductDTO>> GetProductByCategory(int categroyID);


        Task<PagedProductResponseDto> SearchProductsAsync(ProductSearchRequestDto request);

    }
}
