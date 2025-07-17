using OrderProcessingBackEnd.AutoMapper;
using OrderProcessingBackEnd.Data;
using OrderProcessingBackEnd.Repository;
using OrderProcessingBackEnd.DTO;
using OrderProcessingBackEnd.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace OrderProcessingBackEnd.DTO
{
    public class OrderProductDTO
    {
        [Key]
      //  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? OrderProductID { get; set; } // Primary Key (optional if EF auto-handles)

        // Foreign Keys
        [Required]
        public int OrderID { get; set; }

        [Required]
        public int ProductID { get; set; }


        // Additional properties (optional, but professional)
        public int Quantity { get; set; } = 1; // Example
        public decimal PriceAtPurchase { get; set; } // If you want to store snapshot of price

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
