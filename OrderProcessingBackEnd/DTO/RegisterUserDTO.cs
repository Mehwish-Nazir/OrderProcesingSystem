using OrderProcessingBackEnd.AutoMapper;
using OrderProcessingBackEnd.Models;
namespace OrderProcessingBackEnd.DTO
{
    public class RegisterUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; } // Will be hashed before storing
        public string Email { get; set; }
        public string Role { get; set; } // Must be validated,
      
    }
}
