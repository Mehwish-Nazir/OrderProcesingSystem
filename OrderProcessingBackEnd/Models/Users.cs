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
    [Table("Users")]
    public class Users
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; } // Fixed Data Type
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }
        [Required]
        [MaxLength(250)]
        public string PasswordHash { get; set; }
        
        [Required]
        [MaxLength(250)]
        public string Email{ get; set; }
        [Required]
        [MaxLength(50)]
        public string Role { get; set; }  //we create enum here 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ✅ Matches DB
        
        [JsonIgnore] // Avoid circular reference in JSON responses
        public virtual Customers? Customer { get; set; }  // Nullable to avoid constraint issues
        
        
        //Here I didn't use List<Cutomer> bcz user and cuotmer has one to one relationship
         //List<> is used to define one-t- many or many-to-many relationship
    }
    public enum Role
    {
        Customer,
        Admin,
        Employee
    }
}
