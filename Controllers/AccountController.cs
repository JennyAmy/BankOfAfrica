using BankOfAfricaAPI.DTOs.AppUser;
using BankOfAfricaAPI.DTOs.BankDTO;
using BankOfAfricaAPI.Entities;
using BankOfAfricaAPI.Extensions;
using BankOfAfricaAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankOfAfricaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public AccountController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateAppUserDTO createAppUserDTO)
        {
            if (createAppUserDTO.Email.IsEmpty() || createAppUserDTO.Password.IsEmpty())
            {
                return BadRequest("Email or password cannot be empty");
            }

            if (!await unitOfWork.BankAccountRepository.isAccountNumberExisting(createAppUserDTO.AccountNumber))
                return BadRequest("Customer with this account number is not recognised. Please open a bank account to use the app");

            if (await unitOfWork.AppUserRepository.UserAlreadyExists(createAppUserDTO.AccountNumber))
                return BadRequest("User with this account number already exists, please login to your account");
           

            unitOfWork.AppUserRepository.Register(createAppUserDTO);
            await unitOfWork.SaveAsync();
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await unitOfWork.AppUserRepository.Authenticate(loginRequestDTO.Email, loginRequestDTO.Password);
            var userIsValidated = await unitOfWork.AppUserRepository.UserIsValidatedByemail(loginRequestDTO.Email);

            if (user == null)
            {
                return Unauthorized("Invalid Username or Password");
            }

            if(!userIsValidated)
            {
                return Unauthorized("You have not validated your account. Please validate to proceed"); /// Then immeditely route the user to validation page
            }

            var loginResponse = new LoginResponseDTO();
            loginResponse.Firstname = user.Firstname;
            loginResponse.Token = CreateJWT(user);

            return Ok(loginResponse);

        }

        private string CreateJWT(AppUser appUser)
        {
            var secretKey = configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(secretKey));

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, appUser.Firstname),
                new Claim(ClaimTypes.NameIdentifier, appUser.AppUserId.ToString())
            };

            var signingCredentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
