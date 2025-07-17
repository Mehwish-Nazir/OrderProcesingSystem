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
using OrderProcessingBackEnd.Services;
namespace OrderProcessingBackEnd.Services

{

    //define all the methods for login and sign up and implemt them in 
    public interface IUserService
    {
        Task<LoginResponseDTO> AuthenticateUserAsync(LoginUserDTO loginDto);  //login
        Task RegsiterUserAsync(RegisterUserDTO registerDto);  //returns nothing just adds

        Task<UserProfileDTO> GetProfileAsync(int userId);


    }
}
