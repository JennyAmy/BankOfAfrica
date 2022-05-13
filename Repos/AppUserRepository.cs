using BankOfAfricaAPI.DbContexts;
using BankOfAfricaAPI.DTOs.AppUser;
using BankOfAfricaAPI.Entities;
using BankOfAfricaAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BankOfAfricaAPI.Repos
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly DataContext context;

        public AppUserRepository(DataContext context)
        {
            this.context = context;
        }

        public void Register(CreateAppUserDTO createAppUserDTO)
        {
            byte[] passwordHash, passwordKey;

            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(createAppUserDTO.Password));
            }

            AppUser user = new AppUser();
            user.AccountNumber = createAppUserDTO.AccountNumber;
            user.Email = createAppUserDTO.Email;
            user.Firstname = createAppUserDTO.Firstname;
            user.Lastname = createAppUserDTO.Surname;
            user.CustomerId = createAppUserDTO.CustomerId;
            user.Password = passwordHash;
            user.PasswordKey = passwordKey;

            context.AppUsers.Add(user);
        }

        public async Task<AppUser> Authenticate(string email, string passwordText)
        {
            var user = await context.AppUsers.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null || user.PasswordKey == null)
                return null;

            if (!MatchPasswordHash(passwordText, user.Password, user.PasswordKey))
                return null;

            return user;
        }

        private bool MatchPasswordHash(string passwordText, byte[] password, byte[] passwordKey)
        {
            using (var hmac = new HMACSHA512(passwordKey))
            {
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordText));

                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != password[i])
                        return false;
                }
                return true;
            }
        }

        public async Task<bool> UserAlreadyExists(string accountNo) //Might check with account number  ///For someone registering for the first time
        {
            return await context.AppUsers.AnyAsync(x => x.AccountNumber == accountNo);
        }

        public async Task<bool> UserIsValidated(string accountNo)
        {
            return await context.AppUsers.AnyAsync(x => x.AccountNumber == accountNo && x.IsValidated == true);
        }

        public async Task<bool> UserIsValidatedByemail(string email)
        {
            return await context.AppUsers.AnyAsync(x => x.Email == email && x.IsValidated == true);
        }

        public async Task<AppUser> GetUserByAccountNo(string accountNo)
        {
            var user = await context.AppUsers
                .Where(x => x.AccountNumber == accountNo)
                .FirstOrDefaultAsync();

            return user;
        }
        public async Task<AppUser> GetUserByUserId(int userId)
        {
            var user = await context.AppUsers
                .Where(x => x.AppUserId == userId)
                .FirstOrDefaultAsync();

            return user;
        }

    }
}
