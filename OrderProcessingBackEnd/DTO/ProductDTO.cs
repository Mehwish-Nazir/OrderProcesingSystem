using OrderProcessingBackEnd.AutoMapper;
using OrderProcessingBackEnd.Data;
using OrderProcessingBackEnd.Repository;
using OrderProcessingBackEnd.DTO;
using OrderProcessingBackEnd.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.Identity.Client;
namespace OrderProcessingBackEnd.DTO
{
    //this DTO wil be used to fetch product based on categoryID 
    public class ProductDTO
    {
        [Key]
        public int? ProductID { get; set; }
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be gretaer or equal to 0")]
        public Decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock value must be greater or equal to 0")]
        public int Stock { get; set; }

        public DateTime CreatedAt { get; set; }
        [ForeignKey("Category")]
        public int? CategoryID { get; set; }

    }

    public class CreateNewProductDTO
    {
        [Key]
        public int? ProductID { get; set; }
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be gretaer or equal to 0")]
        public Decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock value must be greater or equal to 0")]
        public int Stock { get; set; }

        public int CategoryID { get; set; }
    }

    public class ProductWithCategoryDTO
    {
        [ForeignKey("Category")]
        public CategoryDTO Category { get; set; }
        [Key]
        public int? ProductID { get; set; }
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be gretaer or equal to 0")]
        public Decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock value must be greater or equal to 0")]
        public int Stock { get; set; }

        public DateTime? CreatedAt { get; set; }
        
    }
}
