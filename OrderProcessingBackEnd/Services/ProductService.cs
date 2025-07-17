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

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IRepository<Product> _generalRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Category> _categoryRepository;
        private readonly OrderProcessingDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IProductRepository productRepository, IRepository<Product> generalRepository, IMapper mapper, IRepository<Category> categoryRepository, OrderProcessingDbContext dbContext)
        {
            _productRepository = productRepository;
            _generalRepository = generalRepository ?? throw new ArgumentNullException(nameof(generalRepository));
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _dbContext = dbContext;
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "id must not be negative");
            }

            var product = await _generalRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<List<ProductWithCategoryDTO>> GetProductsWithCategory()
        {
            var products = await _dbContext.Product
                .Include(p => p.Category)
                .ToListAsync();

            return _mapper.Map<List<ProductWithCategoryDTO>>(products);
        }


        public async Task<CreateNewProductDTO> AddProductAsync(CreateNewProductDTO productDTO)
        {
            var existingProduct = await _generalRepository.ProductExistsAsync(productDTO.ProductName, productDTO.CategoryID);
            if (existingProduct)
            {
                throw new InvalidOperationException($"A product with {productDTO.ProductName} already exist");
            }
            if (productDTO == null)
            {
                throw new ArgumentNullException(nameof(productDTO), "product data must be provided");
            }
            var checkCategory = await _categoryRepository.CheckCategoryExistenceByID(productDTO.CategoryID);

            //var checkCategory = await _productRepository.GetByIdAsync(productDTO.CategoryID);
            if (checkCategory == null)
            {
                throw new KeyNotFoundException($"Category with this id {productDTO.CategoryID} not exist");
            }
            if (productDTO.Stock < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(productDTO.Stock), "Stock value can;'t be negative");
            }

            var productEntity = _mapper.Map<Product>(productDTO);
            await _generalRepository.AddAsync(productEntity);
            await _generalRepository.SaveAsync();
            var dtoObject = _mapper.Map<CreateNewProductDTO>(productEntity);
            return dtoObject;
        }

        public async Task<List<ProductDTO>> GetProductByCategory(int categroyID)
        {
            var products = await _generalRepository.GetProductByCategoryID(categroyID);
            if (products == null)
            {
                throw new ArgumentNullException($"This category has no products.");
            }
            var dto = _mapper.Map<List<ProductDTO>>(products);
            return dto;
        }


        public async Task<PagedProductResponseDto> SearchProductsAsync(ProductSearchRequestDto request)
        {
            try
            {
                return await _productRepository.SearchProductsAsync(request);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid search parameters.");
                throw;
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Repository error while searching products.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in ProductService.");
                throw new ApplicationException("An unexpected error occurred in the product search service.", ex);
            }
        }
    }
}
