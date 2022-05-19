using AutoMapper;
using BankOfAfricaAPI.DTOs.BankDTO;
using BankOfAfricaAPI.Entities;
using BankOfAfricaAPI.Extensions;
using BankOfAfricaAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BankOfAfricaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public BankAccountController(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount(BankAccountDTO bankAccountDTO)
        {
            var account = mapper.Map<BankAccount>(bankAccountDTO);

            account.AccountNumber = unitOfWork.BankAccountRepository.GenerateAccountNumber();
            account.AccountOfficerEmail = "jenniferoliseyenum@bankofafrica.com";
            account.AccountOfficerName = "Jennifer Oliseyenum";

            //if (account != null)
            //{
            //    return BadRequest("Please fill in all compulsory fields");
            //}

            if (await unitOfWork.BankAccountRepository.BVNAlreadyExists(account.BVN))
                return BadRequest("A bank account with this BVN already exists. Please check and try again");

            if (await unitOfWork.BankAccountRepository.NINAlreadyExists(account.NIN))
                return BadRequest("A bank account with this NIN already exists. Please check and try again");

            unitOfWork.BankAccountRepository.CreateAccount(account);

            if (await unitOfWork.SaveAsync()) 
                return Ok("Welcome to Bank of Africa! Your account number is " + account.AccountNumber);

            return BadRequest();
        }


        [HttpGet("get-details-accountNo/{accountNo}")]
        public async Task<IActionResult> GetDetailsByAccountNumber(string accountNo)
        {
            var isAccountExisiting = await unitOfWork.BankAccountRepository.isAccountNumberExisting(accountNo);

            if (isAccountExisiting == true)
            {
                var details = await unitOfWork.BankAccountRepository.GetAccountDetailsByAccountNo(accountNo);
                var userDetails = mapper.Map<BankAccountDTO>(details);
                return Ok(userDetails);
            }

            return BadRequest("Account number: " + accountNo + " does not exist. Please open a bank account to access the internet banking app");

        }


        [HttpGet("get-details")]
        public async Task<IActionResult> GetDetailsById()
        {
            var loggedInUser = GetUserId();
            var customer = await unitOfWork.AppUserRepository.GetUserByUserId(loggedInUser);

            var details = await unitOfWork.BankAccountRepository.GetAccountDetailsById(customer.CustomerId);
            var userDetails = mapper.Map<BankAccountDTO>(details);
            return Ok(userDetails);


        }

        [HttpGet("get-accountofficer")]
        public async Task<IActionResult> GetAccountOfficer()
        {
            var loggedInUser = GetUserId();
            var customer = await unitOfWork.AppUserRepository.GetUserByUserId(loggedInUser);
            var accountOfficer = await unitOfWork.BankAccountRepository.GetAccountOfficerByUserId(customer.CustomerId);
                var userDetails = mapper.Map<BankAccountDTO>(accountOfficer);
                return Ok(userDetails);
        }

        [HttpPost("account-exists")] //To check if an acccount number exists
        public async Task<IActionResult> AccountExists(string accountNo)
        {
            var isAccountExisiting = await unitOfWork.BankAccountRepository.isAccountNumberExisting(accountNo);

            return isAccountExisiting == true ? StatusCode(200) : BadRequest("Account number does not exist. Please open a bank account to access the internet banking app");

        }

        [HttpPost("is-email-exists")]
        public async Task<IActionResult> ConfirmEmailExists(BankAccountDTO bankAccountDTO)  //To check if an email exists on the fly -- Immediately after input is made
        {
            var isEmailExisting = await unitOfWork.BankAccountRepository.ConfirmExistingEmail(bankAccountDTO.Email, bankAccountDTO.CustomerId);

            return isEmailExisting == true ? StatusCode(200) : BadRequest("User with this email does not exist. Please open a bank account to access the internet banking app");

        }
    }
}
