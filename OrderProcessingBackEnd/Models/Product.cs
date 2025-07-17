using Microsoft.Identity.Client;
using OrderProcessingBackEnd.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OrderProcessingBackEnd.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage ="Price must be gretaer or equal to 0")]
        public Decimal Price { get; set; }

        [Required]
        [Range(0,int.MaxValue, ErrorMessage ="Stock value must be greater or equal to 0")]
        public int Stock { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        [JsonIgnore]

        //one product can belong to one category
       public  virtual Category? Category { get; set; }
        //one product can contanin many rders  and one order can contain many Produts 

        //public virtual List<Orders> Orders { get; set; } = new List<Orders>();

        //as both product and order has many to many relation ship so need to create sepeare model aND table to join them 
        //And add common tbale in both Product and Order mdoel navigation

        public virtual List<OrderProducts> OrderProducts { get; set; } = new List<OrderProducts>();


    }
}
