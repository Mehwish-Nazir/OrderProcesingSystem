using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OrderProcessingBackEnd.AutoMapper;
using OrderProcessingBackEnd.Models;
using OrderProcessingBackEnd.DTO;
using OrderProcessingBackEnd.Data;
using OrderProcessingBackEnd.Repository;
using OrderProcessingBackEnd.Services;
using OrderProcessingBackEnd.Controllers;
using System.Text.Json.Serialization;
namespace OrderProcessingBackEnd.DTO
{
    public class CategoryDTO
    {
         [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [JsonIgnore] // optional: prevents binding in DTO
        public int CategoryID { get; set; }


        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }

    }
    
    public class CreateProductDTO   //creating this dto to inject in 'CreateCategoryWithProductsDTO'
    {
        /* [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int ProductID{ get; set; }*/
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be gretaer or equal to 0")]
        public Decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock value must be greater or equal to 0")]
        public int Stock { get; set; }
    }
    public class CreateCategoryWithProductsDTO
    {
      /*  [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [JsonIgnore] // optional: prevents binding in DTO
        public int CategoryID { get; set; }*/
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        public List<CreateProductDTO> Product { get; set; } = new(); //to add multiple products in one category
    }

    public class UpdateCategoryWithProductDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [JsonIgnore] // optional: prevents binding in DTO as it is auto genrated and will not be included in swagger UI request body
        public int CategoryID { get; set; }


        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        public List<Product>? Product { get; set; }
    }
}
