using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using OrderProcessingBackEnd.Models;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration.UserSecrets;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;

namespace OrderProcessingBackEnd.Models
{
    [Table("Customers")]
    public class Customers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }
        [Required]
        //one to one relationship
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("User")]
        public int? UserID { get; set; }

        [JsonIgnore]
        public virtual Users? User { get; set; }

        // One-to-Many Relationship: One Customer → Many Orders
        public virtual List<Orders> Order { get; set; } = new List<Orders>();

    }
}
