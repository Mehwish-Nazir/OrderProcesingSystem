using OrderProcessingBackEnd.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace OrderProcessingBackEnd.DTO
{
    public class CustomerDto
    {
        [Key]
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
        //public DateTime? CreatedAt { get; set; }
        // public string UserId { get; set; }
        //public int? UserID { get; set; }
        [ForeignKey("Users")]
        public int UserID { get; set; }

    }
}
