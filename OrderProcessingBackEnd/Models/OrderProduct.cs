using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using OrderProcessingBackEnd.Data;
using OrderProcessingBackEnd.Models;
using OrderProcessingBackEnd.AutoMapper;
using OrderProcessingBackEnd.DTO;

namespace OrderProcessingBackEnd.Models
{
    //Common Join tabe for both Orders and Product table as both have many to many relationshp
    [Table("OrderProducts")] // ← Use the real table name from your DB

    public class OrderProducts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderProductID { get; set; } // Primary Key (optional if EF auto-handles)

        // Foreign Keys
        [ForeignKey("Orders")]
        [Required]
        public int OrderID { get; set; }

        [ForeignKey("Product")]
        [Required]
        public int ProductID { get; set; }
         
        // Navigation properties (many to many)
        public virtual Orders Order { get; set; }
        public virtual Product Product { get; set; }

        // Additional properties (optional, but professional)
        public int Quantity { get; set; } = 1; // Example
        public decimal PriceAtPurchase { get; set; } // If you want to store snapshot of price

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
