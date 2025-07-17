using OrderProcessingBackEnd.Models;
namespace OrderProcessingBackEnd.DTO
{
    public class LoginUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public bool HasCustomerProfile { get; set; }
    }
    public class UserProfileDTO
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; }
        // Optional fields for future
        // public string Email { get; set; }
        // public string ProfileImageUrl { get; set; }
    }
}
