using System.Threading.Tasks;

namespace BankOfAfricaAPI.Interfaces
{
    public interface IUnitOfWork
    {
        IBankAccountRepository BankAccountRepository { get; }
        IAppUserRepository AppUserRepository { get; }

        Task<bool> SaveAsync();
    }
}
