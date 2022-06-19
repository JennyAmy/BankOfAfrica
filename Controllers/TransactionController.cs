using AutoMapper;
using BankOfAfricaAPI.DTOs.BankDTO;
using BankOfAfricaAPI.Entities;
using BankOfAfricaAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BankOfAfricaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : BaseController
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public TransactionController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }


       
        [HttpPut("transfer/{amount}/{accountNo}")]
        public async Task<IActionResult> Transfer(decimal amount, string accountNo, TransactionDTO transactionDTO)
        {
            var loggedInUserId = GetUserId();
            var isAccountExisiting = await unitOfWork.BankAccountRepository.isAccountNumberExisting(accountNo);
            var customer = await unitOfWork.AppUserRepository.GetUserByUserId(loggedInUserId);
            var sender = await unitOfWork.BankAccountRepository.GetAccountDetailsById(customer.CustomerId);
            var receiver = await unitOfWork.BankAccountRepository.GetAccountDetailsByAccountNo(accountNo);
            var receiverAppId = await unitOfWork.AppUserRepository.GetUserByAccountNo(accountNo);
            var transaction = mapper.Map<Transaction>(transactionDTO);

            if (isAccountExisiting)
            {
                if (accountNo != sender.AccountNumber)
                {
                    if (amount < sender.AccountBal) //Check if the sender's account bal is less than amount about to be sent
                    {
                        //First debit the user
                        sender.AccountBal = sender.AccountBal - amount;
                        sender.LastUpdatedBy = loggedInUserId;
                        sender.LastUpdatedOn = DateTime.Now;

                        //Then credit the user
                        receiver.AccountBal = receiver.AccountBal + amount;
                        receiver.LastUpdatedOn = DateTime.Now;
                        receiver.LastUpdatedBy = loggedInUserId;

                        //Log the transaction in transactions table
                        transaction.SenderId = loggedInUserId;
                        transaction.ReceiverId = receiverAppId.AppUserId;
                        transaction.SenderAccountNo = sender.AccountNumber;
                        transaction.ReceiverAccountNo = accountNo;
                        transaction.AmountSent = amount;
                        transaction.ReferenceNumber = unitOfWork.BankAccountRepository.GenerateReferenceNumber();

                        //mapper.Map(transactionDTO, transaction);
                        unitOfWork.BankAccountRepository.AddTransaction(transaction);
                    }
                    else
                    {
                        return BadRequest(new { status = false, message = "Insufficient balance to carry out this transaction" });
                    }
                }
                else
                {
                    return BadRequest(new { status = false, message = "You cannot transfer money to your own account" });
                }
                await unitOfWork.SaveAsync();
                return Ok(new { status = true, message = "You have successfully transferred " + amount + " to " + receiver.Firstname + " " + receiver.Surname });
            }
          return BadRequest(new { status = false, message = "Invalid Account number" });


        }

        [HttpGet("get-debit-details")]   ///Refactor to a single endpoint. Use unique characteristics to filter and display accordingly
        public async Task<IActionResult> GetDebitDetailsById()
        {
            var loggedInUser = GetUserId();

            var details = await unitOfWork.BankAccountRepository.GetDebitDetailsById(loggedInUser);
            var debitDetails = mapper.Map<TransactionDTO>(details);
            return Ok(new { status = true, data = debitDetails });
        }

        [HttpGet("get-credit-details")]
        public async Task<IActionResult> GetCreditDetailsById()
        {
            var loggedInUser = GetUserId();

            var details = await unitOfWork.BankAccountRepository.GetCreditDetailsById(loggedInUser);
            var creditDetails = mapper.Map<TransactionDTO>(details);
            return Ok(new { status = true, data = creditDetails });


        }

    }
}
