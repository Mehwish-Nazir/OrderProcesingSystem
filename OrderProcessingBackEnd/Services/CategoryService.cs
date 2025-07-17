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
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        

        public CategoryService(IRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;

        }
        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {

            var category = await _categoryRepository.GetByIdAsync(id);
            if (id <= 0)
            {
                throw new ArgumentException("Invalid customr ID , Id must be greate than 0");
            }
            if (category == null)
            {
                return null;
            }
            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            return categoryDTO;
        }



        public async Task<List<CategoryDTO>> GetAllCategoryAsync()
        {
            var categories = await _categoryRepository.GetAllAsync(); //  No page or pageSize

            return _mapper.Map<List<CategoryDTO>>(categories);
        }



        public async Task<CategoryDTO> GetCategoryWithProductsAsync(int categoryId)
        {
            //in include use Entity not Dto 
            var includes = new List<Expression<Func<Category, object>>>
    {
        c => c.Product
    };

            return await _categoryRepository.GetEntityWithRelatedDataAsync<CategoryDTO>(
                categoryId,
                filter: c => c.CategoryID == categoryId,
                includes: includes
            );
        }


        public async Task<CategoryDTO> AddCategoryAsync(CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                throw new ArgumentNullException(nameof(categoryDTO));
            }
            var exists = await _categoryRepository.GetByIdAsync(categoryDTO.CategoryID);
            
            if (exists != null)
            {
                throw new InvalidOperationException($"A category with this id {categoryDTO.CategoryID} already exists.");
            }
            Category category;
            try
            {
                //check dto and model match
                category = _mapper.Map<Category>(categoryDTO);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Mapping failed: " + ex.Message);

            }
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveAsync();
            //convert Entity to DTO 
            var resultDto = _mapper.Map<CategoryDTO>(category);
            return resultDto;
        }

        public async Task<CreateCategoryWithProductsDTO> AddCategoryWithProductAsync(CreateCategoryWithProductsDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var exists = await _categoryRepository.GetByName(dto.CategoryName);
            if (exists)
            {
                throw new InvalidOperationException($"A category with this name '{dto.CategoryName}' already exists.");
            }

            Category categoryEntity;

            try
            {
                // 1. Mapping
                try
                {
                    categoryEntity = _mapper.Map<Category>(dto);

                    // Set navigation properties explicitly (important if EF is not handling this automatically)
                    if (categoryEntity.Product != null)
                    {
                        foreach (var product in categoryEntity.Product)
                        {
                            product.Category = categoryEntity;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Mapping failed: " + ex.Message, ex);
                }

                // 2. Save to database
                try
                {
                    await _categoryRepository.AddAsync(categoryEntity);
                    await _categoryRepository.SaveAsync();
                }
                catch (Exception ex)
                {
                    var detailedError = ex.ToString(); // Get full exception details
                   // _logger.LogError($"Error while saving entity: {detailedError}");
                    throw new InvalidOperationException("Saving to DB failed", ex);
                }

                // 3. Return result
                var resultDto = _mapper.Map<CreateCategoryWithProductsDTO>(categoryEntity);
                return resultDto;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while adding category with product.", ex);
            }
        }



        public async Task<CategoryDTO> UpdateCategoryAsync(int id, CategoryDTO categoryDTO)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Inavlid Id , Id must be gretae than 0 or positive number");
            }

            var exists = await _categoryRepository.GetByIdAsync(categoryDTO.CategoryID);
            if (exists == null)
            {
                throw new InvalidOperationException($"Category with this id '{categoryDTO.CategoryID}'");
            }
            // Step 3: Map the updated DTO to the entity

            var categoryEntity = _mapper.Map<Category>(categoryDTO);

            // Step 4: Preserve fields that should not be updated (e.g., CreatedAt)
          //  categoryEntity.CreatedAt = (DateTime)categoryDTO.CreatedAt;  // Preserve original creation date

            // Step 5: Update the category entity in the database
            await _categoryRepository.UpdateAsync(categoryEntity);
            // Step 6: Map and return the updated category

            var updatedCategoryDTO = _mapper.Map<CategoryDTO>(categoryEntity);
            return updatedCategoryDTO;
        }


        


        public async Task<CategoryDTO> UpdateCategoryAlongProductAsync(int id, UpdateCategoryWithProductDTO categoryDTO)
        {
            var category = await _categoryRepository.GetEntityWithRelatedDataAsync<Category>(
            id,
            filter: c => c.CategoryID == id, // Filter by category ID
            includes: new List<Expression<Func<Category, object>>> { c => c.Product } // Include related Products
            );

            if (category == null || id != categoryDTO.CategoryID)
            {
                // If no category is found, you can return null or throw an exception
                throw new ArgumentException("Category ID mismatch or null data.");
            }

            var categoryEntity = _mapper.Map<Category>(categoryDTO);
            if (categoryEntity == null)
            {
                throw new InvalidOperationException($"No category found with ID {id}");
            }
            categoryEntity.CategoryID = categoryDTO.CategoryID;
            categoryEntity.CategoryName = categoryDTO.CategoryName;

            foreach (var productDTO in categoryDTO.Product)
            {
                var product = category.Product.FirstOrDefault(p => p.ProductID == productDTO.ProductID);

                if (product != null)
                {
                    // If the product exists, update it using AutoMapper
                    _mapper.Map(productDTO, product);
                }
                else
                {
                    // If the product doesn't exist, create a new Product and map from DTO
                    var newProduct = _mapper.Map<Product>(productDTO);
                    category.Product.Add(newProduct);
                }
            }
            await _categoryRepository.UpdateAsync(category);

            // Step 5: Return the updated CategoryDTO
            return _mapper.Map<CategoryDTO>(category);


        }



    }
}
