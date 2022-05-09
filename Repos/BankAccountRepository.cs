using AutoMapper;
using BankOfAfricaAPI.DbContexts;
using BankOfAfricaAPI.DTOs.BankDTO;
using BankOfAfricaAPI.Entities;
using BankOfAfricaAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BankOfAfricaAPI.Repos
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public BankAccountRepository(DataContext context)
        {
            this.context = context;
        }

        public BankAccountRepository(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public void CreateAccount(BankAccount bankAccount)
        {
            context.BankAccounts.Add(bankAccount);
        }

        public async Task<bool> NINAlreadyExists(string nin)
        {
            return await context.BankAccounts.AnyAsync(x => x.NIN == nin);
        }

        public async Task<bool> BVNAlreadyExists(string bvn)
        {
            return await context.BankAccounts.AnyAsync(x => x.BVN == bvn);
        }

        public bool CardIsValid(int cardDgits)
        {
            if(cardDgits == 1234)
            {
                return true;
            }
            return false;

        }

        public async Task<bool> ConfirmExistingEmail(string email, int customerId)
        {
            return await context.BankAccounts.AnyAsync(x => x.Email == email && x.CustomerId == customerId);
        }

        public async Task<bool> isAccountNumberExisting(string accountNo)
        {
            return await context.BankAccounts.AnyAsync(x => x.AccountNumber == accountNo);
        }

        public async Task<BankAccount> GetAccountDetailsByAccountNo(string accountNo)
        {
            var details = await context.BankAccounts
                .Where(x => x.AccountNumber == accountNo)
                .FirstOrDefaultAsync();

            return details;
        }

        public async Task<BankAccount> GetAccountDetailsById(int customerId)
        {
            var details = await context.BankAccounts
                .Where(x => x.CustomerId == customerId)
                .FirstOrDefaultAsync();

            return details;
        }

        public async Task<BankAccount> GetAccountOfficerByUserId(int customerId)
        {
            var details = await context.BankAccounts
                .Where(x => x.CustomerId == customerId)
                .Select(x => new BankAccount { AccountOfficerEmail = x.AccountOfficerEmail, AccountOfficerName = x.AccountOfficerName })
                .FirstOrDefaultAsync();

            return details;
        }

        public void AddTransaction(Transaction transaction)
        {
            context.Transactions.Add(transaction);
        }


        public string GenerateAccountNumber()
        {
            
            if (!context.BankAccounts.Any())
            {
                string accountNumber;
                string begin = "00";
                Random random = new Random();
                accountNumber = begin + random.Next(0, 99999999).ToString();
                return accountNumber;
            }
            else
            {
                int maxID = context.BankAccounts.Max(acc => acc.CustomerId);
                var account = context.BankAccounts.FirstOrDefault(ac => ac.CustomerId == maxID);
               
                long lastAccountNumber, newAccountNumber;
                string stringnewAccountNumber;
                try
                {
                    lastAccountNumber = long.Parse(account.AccountNumber);
                    newAccountNumber = lastAccountNumber + 1;
                    stringnewAccountNumber = newAccountNumber.ToString("D10");


                }
                catch (Exception error)
                {

                    throw new Exception(error.Message);
                }
                return stringnewAccountNumber;
            }


        }
    }
}
