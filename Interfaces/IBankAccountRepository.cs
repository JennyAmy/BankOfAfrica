using BankOfAfricaAPI.DTOs.BankDTO;
using BankOfAfricaAPI.Entities;
using System.Threading.Tasks;

namespace BankOfAfricaAPI.Interfaces
{
    public interface IBankAccountRepository
    {
        void AddTransaction(Transaction transaction);
        Task<bool> BVNAlreadyExists(string bvn);
        bool CardIsValid(int cardDgits);
        Task<bool> ConfirmExistingEmail(string email, int customerId);
        void CreateAccount(BankAccount createAccount);
        string GenerateAccountNumber();
        string GenerateReferenceNumber();
        Task<BankAccount> GetAccountDetailsByAccountNo(string accountNo);
        Task<BankAccount> GetAccountDetailsById(int customerId);
        Task<BankAccount> GetAccountOfficerByUserId(int customerId);
        Task<Transaction> GetCreditDetailsById(int appUserId);
        Task<Transaction> GetDebitDetailsById(int appUserId);
        Task<bool> isAccountNumberExisting(string accountNo);
        Task<bool> NINAlreadyExists(string nin);
    }
}
