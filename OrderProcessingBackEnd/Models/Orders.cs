using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using OrderProcessingBackEnd.Models;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration.UserSecrets;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace OrderProcessingBackEnd.Models
{
    public class Orders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        //[Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "TotalAmount must be a non-negative value.")]  //rnage starting from 0 to max value
        public decimal TotalAmount { get; set; }
        [Required]
        [MaxLength(50)]
        public string OrderStatus { get; set; }  //create enum for order status below 

        [Required]
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        [JsonIgnore]
        public virtual Customers? Customer { get; set; }
        public virtual List<Transactions> Transaction { get; set; } = new List<Transactions>();
        
        //as both product and order has many to many relation ship so need to create sepeare model aND table to join them 
        //And add common tbale in both Product and Order mdoel navigation
        public virtual List<OrderProducts> OrderProducts { get; set; } = new List<OrderProducts>();

    }
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}


//where to use 'new ' and where not 
//General Rule
/*Type	         Example	                                                                          Use new?         Why ?
One - to - Many public virtual List<Transaction> Transactions { get; set; } = new List<Transaction>();	✅ Yes Prevents null errors when adding items.
Many-to-One	public virtual Customers Customer { get; set; }	❌ No EF Core assigns the related object.
Relationship Type | Navigation Property         | Use new?  | Why?
One-to-Many | public virtual List<T> Collection | ✅ Yes | To avoid null reference when adding items (EF does not initialize).
Many-to-One | public virtual T Object | ❌ No | EF Core sets this value during query (don’t override it yourself).
One-to-One (optional) | public virtual T? Object | ❌ No | EF will manage this for you.
*/
