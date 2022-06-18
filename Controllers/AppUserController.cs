using AutoMapper;
using BankOfAfricaAPI.DTOs.AppUser;
using BankOfAfricaAPI.DTOs.BankDTO;
using BankOfAfricaAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankOfAfricaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public AppUserController(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
            this.mapper = mapper;
        }


        [HttpPost("user-exists")] //To check if a user exists on the fly -- Immediately after input is made
        public async Task<IActionResult> UserExists(string accountNo)
        {
            var isUserExisiting = await unitOfWork.AppUserRepository.UserAlreadyExists(accountNo);

            return isUserExisiting ? BadRequest("A user with this account number already exists. Please login") : StatusCode(200);

        }

        [HttpPost("validate-account")] //To check if a user exists on the fly -- Immediately after input is made
        public async Task<IActionResult> ValidateAccount(string accountNo)
        {
            var user = await unitOfWork.AppUserRepository.GetUserByAccountNo(accountNo);
            var userIsValidated = await unitOfWork.AppUserRepository.UserIsValidated(accountNo);
            var isUserExisiting = await unitOfWork.AppUserRepository.UserAlreadyExists(accountNo);

            if (isUserExisiting == true)
            {
                if (!userIsValidated)
                {
                    user.IsValidated = true;
                }
                else
                {
                    return BadRequest(new { status = false, message = "This account has already been validated. Please login" });
                }
            }
            else
            {
                return NotFound(new { status = false, message = "User not found. Please create an account" });
            }

            await unitOfWork.SaveAsync();
            return Ok(new { status = false, message = true});

        }

        //[HttpGet("users")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetUsers()
        //{
        //    var users = await unitOfWork.AppUserRepository.GetUsers();

        //    var usersDTO = mapper.Map<IEnumerable<CreateAppUserDTO>>(users);
        //    return Ok(usersDTO);
        //}

    }
}
