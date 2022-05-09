using AutoMapper;
using BankOfAfricaAPI.DbContexts;
using BankOfAfricaAPI.Entities;
using BankOfAfricaAPI.Interfaces;

namespace BankOfAfricaAPI.Repos
{
    public class TransactionRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public TransactionRepository(DataContext context, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void AddTransaction(Transaction transaction)
        {
            context.Transactions.Add(transaction);
        }
    }
}
