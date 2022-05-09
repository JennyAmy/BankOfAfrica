using BankOfAfricaAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankOfAfricaAPI.DbContexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }
}
