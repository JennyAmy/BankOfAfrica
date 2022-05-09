using AutoMapper;
using BankOfAfricaAPI.DTOs.BankDTO;
using BankOfAfricaAPI.Entities;

namespace BankOfAfricaAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<BankAccount, BankAccountDTO>().ReverseMap();
            CreateMap<Transaction, TransactionDTO>().ReverseMap();
        }
    }
}
