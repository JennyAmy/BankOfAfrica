using BankOfAfricaAPI.DbContexts;
using BankOfAfricaAPI.Interfaces;
using System.Threading.Tasks;

namespace BankOfAfricaAPI.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext context;

        public UnitOfWork(DataContext context)
        {
            this.context = context;
        }

        public IBankAccountRepository BankAccountRepository =>
            new BankAccountRepository(context);

        public IAppUserRepository AppUserRepository =>
           new AppUserRepository(context);

        public async Task<bool> SaveAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
