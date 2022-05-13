using BankOfAfricaAPI.DTOs.AppUser;
using BankOfAfricaAPI.Entities;
using System.Threading.Tasks;

namespace BankOfAfricaAPI.Interfaces
{
    public interface IAppUserRepository
    {
        Task<AppUser> Authenticate(string email, string passwordText);
        Task<bool> UserIsValidated(string accountNo);
        void Register(CreateAppUserDTO createAppUserDTO);
        Task<bool> UserAlreadyExists(string accountNo);
        Task<AppUser> GetUserByAccountNo(string accountNo);
        Task<bool> UserIsValidatedByemail(string email);
        Task<AppUser> GetUserByUserId(int userId);
        //Task<bool> ValidateAccount(string bvn, int cardDigits);
    }
}
