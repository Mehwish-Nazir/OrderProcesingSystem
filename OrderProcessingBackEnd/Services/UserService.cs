using OrderProcessingBackEnd.Services;
using OrderProcessingBackEnd.DTO;
using OrderProcessingBackEnd.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderProcessingBackEnd.Data;
using BC = BCrypt.Net.BCrypt;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Numerics;
using OrderProcessingBackEnd.Repository;
namespace OrderProcessingBackEnd.Services
{
    public class UserService : IUserService
    {
        private readonly OrderProcessingDbContext _context;
        private readonly IConfiguration _config;
        private readonly IRepository<Users> _userRepository;
        public UserService(IRepository<Users> userRepository,OrderProcessingDbContext context, IConfiguration config)
        {
            _userRepository = userRepository;
            _context = context;
            _config = config;
        }

        public async Task<LoginResponseDTO> AuthenticateUserAsync(LoginUserDTO loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Customer)
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null || !BC.Verify(loginDto.Password, user.PasswordHash))
                throw new Exception("Invalid username or password");

            // ✅ Generate token
            var token = GenerateJwtToken(user);

            return new LoginResponseDTO
            {
                Token = token,
                HasCustomerProfile = user.Role != "Customer" || user.Customer != null
            };
        }


        public async Task RegsiterUserAsync(RegisterUserDTO registerDto)
        {
            //var name = await _context.Users.AnyAsync(u => u.Username == registerDto.Username);
            
            //use bool to check whter username exist or not 
            bool userExists = await _context.Users.AnyAsync(u => u.Username == registerDto.Username);
            if (userExists)
            {
                throw new Exception("Username already exist");
            }
           
            // Hash the password before storing
            string hashedPassword = BC.HashPassword(registerDto.Password);

            //add data in database 
            var user = new Users
            {
                Username = registerDto.Username,
                PasswordHash = hashedPassword,
                Email = registerDto.Email,
                Role = registerDto.Role
            };
            await _context.Users.AddAsync(user);  //to save regitered data in db 
            await _context.SaveChangesAsync();
           
            

           
        }
        // GENERATE JWT TOKEN
        private string GenerateJwtToken(Users user)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var claims = new List<Claim>
            {
                //new Claim("CustomerID", user.Customer.CustomerID.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),

               // new Claim(ClaimTyoes.CustomerName, user.CustomerName),  //not suitable to use 'ClaimTypes' for custome claims but if direclty use user.CustomerName use claimTypes
            
            // new Claim("customerName", user.Customer.FirstName+user.Customer.LastName) //customer claim 
            /* 1️⃣ 'Passwords' Should NEVER Be Stored in JWT(Security Risk!)
Tokens can be intercepted if not properly secured.

If an attacker gets the JWT, they will see the password in plain text.

Passwords should be securely hashed and stored only in the database.*/
        };
            //added custo claim using List<claim > on above
            //added custo claim using List<claim > on above
            if (user.Customer != null)
            {
                var fullName = $"{user.Customer.FirstName} {user.Customer.LastName}";
                claims.Add(new Claim("customerName", fullName));
            }

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //Get User Profile 
        public async Task<UserProfileDTO> GetProfileAsync(int userId)
        {
            return await _userRepository.GetDisplayNameWithRoleByUserIdAsync(userId);
        }

    }
}
