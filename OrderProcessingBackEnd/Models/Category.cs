using OrderProcessingBackEnd.AutoMapper;
using OrderProcessingBackEnd.Data;
using OrderProcessingBackEnd.Repository;
using OrderProcessingBackEnd.DTO;
using OrderProcessingBackEnd.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace OrderProcessingBackEnd.Models
{
    //register this mdoel in dbContext file
    public class Category  
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]

        //one= to many relationship
        virtual public List<Product> Product { get; set; } = new List<Product>();

    }
}
