using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QienUrenMVC.Data;
using QienUrenMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMVC.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext repositoryContext;
        private readonly UserManager<UserIdentity> _userManager;

        public AccountRepository(UserManager<UserIdentity> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            this.repositoryContext = context;
        }

        public async Task<List<AccountModel>> GetAllAccounts(string searchString)
        {
            var allAccounts = new List<AccountModel>();
            foreach (var account in await repositoryContext.UserIdentity.Where(
                                                                            x=>x.FirstName.Contains(searchString) || x.LastName.Contains(searchString) || searchString == null
                                                                          ).ToListAsync())

                allAccounts.Add(new AccountModel
                {
                    AccountId = account.Id,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    Email = account.Email,
                    DateOfBirth = account.DateOfBirth,
                    Address = account.Address,
                    ZIP = account.ZIP,
                    MobilePhone = account.PhoneNumber,
                    City = account.City,
                    IBAN = account.IBAN,
                    CreationDate = account.CreationDate,
                    ProfileImage = account.ProfileImage,
                    IsAdmin = account.IsAdmin,
                    IsActive = account.IsActive,
                    IsQienEmployee = account.IsQienEmployee,
                    IsSeniorDeveloper = account.IsSeniorDeveloper,
                    IsTrainee = account.IsTrainee
                }) ;

            return allAccounts;
        }

        public async Task<AccountModel> AddNewAccount(AccountModel account)
        {

            
            UserIdentity accountEntity = new UserIdentity
            {
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email,
                DateOfBirth = account.DateOfBirth,
                Address = account.Address,
                ZIP = account.ZIP,
                PhoneNumber = account.MobilePhone,
                City = account.City,
                IBAN = account.IBAN,
                CreationDate = DateTime.Now,
                ProfileImage = account.ProfileImage,
                IsAdmin = account.IsAdmin,
                IsActive = account.IsActive,
                IsQienEmployee = account.IsQienEmployee,
                IsSeniorDeveloper = account.IsSeniorDeveloper,
                IsTrainee = account.IsTrainee,
                UserName = account.Email,
                EmailConfirmed = true,
                LockoutEnabled = true
                //SecurityStamp
                //ConcurrencyStamp
                //PhoneNumberConfirmed
                //TwoFactorEnabled
                //LockoutEnd
                //AccessFailedCount
                //Discriminator

            };

            var result = await _userManager.CreateAsync(accountEntity, account.HashedPassword);
            //repositoryContext.UserIdentity.Add(accountEntity);


            await repositoryContext.SaveChangesAsync();
            account.AccountId = accountEntity.Id;
            return account;


        }

        public string RemoveAccount(string accountId)
        {
            var account = repositoryContext.UserIdentity.SingleOrDefault(p => p.Id == accountId);
            repositoryContext.UserIdentity.Remove(account);
            repositoryContext.SaveChanges();
            return "Record has succesfully Deleted";

        }

        public async Task<AccountModel> ModifyAccountActivity(string accountId, bool IsActive)
        {
            UserIdentity account = await repositoryContext.UserIdentity.SingleOrDefaultAsync(p => p.Id == accountId);

            account.IsActive = IsActive;

            await repositoryContext.SaveChangesAsync();

            return new AccountModel
            {
                AccountId = account.Id,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email,
                DateOfBirth = account.DateOfBirth,
                Address = account.Address,
                ZIP = account.ZIP,
                MobilePhone = account.PhoneNumber,
                City = account.City,
                IBAN = account.IBAN,
                CreationDate = account.CreationDate,
                ProfileImage = account.ProfileImage,
                IsAdmin = account.IsAdmin,
                IsActive = account.IsActive,
                IsQienEmployee = account.IsQienEmployee,
                IsSeniorDeveloper = account.IsSeniorDeveloper,
                IsTrainee = account.IsTrainee
            };

        }

        public async Task<AccountModel> GetOneAccount(string accountId)
        {
            UserIdentity account = await repositoryContext.UserIdentity.SingleAsync(p => p.Id == accountId);

            return new AccountModel
            {
                AccountId = account.Id,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email,
                DateOfBirth = account.DateOfBirth,
                Address = account.Address,
                ZIP = account.ZIP,
                MobilePhone = account.PhoneNumber,
                City = account.City,
                IBAN = account.IBAN,
                CreationDate = account.CreationDate,
                ProfileImage = account.ProfileImage,
                IsAdmin = account.IsAdmin,
                IsActive = account.IsActive,
                IsQienEmployee = account.IsQienEmployee,
                IsSeniorDeveloper = account.IsSeniorDeveloper,
                IsTrainee = account.IsTrainee
            };
        }

        public async Task<AccountModel> UpdateAccount(AccountModel account)
        {
            UserIdentity entity = repositoryContext.UserIdentity.Single(p => p.Id == account.AccountId);
            entity.FirstName = account.FirstName;
            entity.LastName = account.LastName;
            entity.Address = account.Address;
            entity.City = account.City;
            entity.CreationDate = account.CreationDate;
            entity.DateOfBirth = account.DateOfBirth;
            entity.Email = account.Email;
            entity.IBAN = account.IBAN;
            entity.ProfileImage = account.ProfileImage;
            entity.ZIP = account.ZIP;
            entity.IsActive = account.IsActive;
            entity.IsAdmin = account.IsAdmin;
            entity.IsQienEmployee = account.IsQienEmployee;
            entity.IsSeniorDeveloper = account.IsSeniorDeveloper;
            entity.PhoneNumber = account.MobilePhone;
            entity.IsTrainee = account.IsTrainee;

            await repositoryContext.SaveChangesAsync();

            return account;
        }


        public async Task<List<AccountModel>> getPersonaliaFromAccount(string accountId)
        {
            var personaliaEnitities = await repositoryContext.UserIdentity.Where(p => p.Id == accountId).ToListAsync();
            List<AccountModel> PersonaliaPerUser = new List<AccountModel>();

            foreach (var personalia in personaliaEnitities)
            {
                PersonaliaPerUser.Add(new AccountModel
                {
                    AccountId = personalia.Id,
                    FirstName = personalia.FirstName,
                    LastName = personalia.LastName,
                    Address = personalia.Address,
                    ZIP = personalia.ZIP,
                    City = personalia.City

                });
            }
            return PersonaliaPerUser;
        }

        public async Task<List<string>> GetAccountIdsByRole(string role)
        {
            List<string> ids = new List<string>();

            if (role == "Trainee") {
                ids = await repositoryContext.UserIdentity.Where(p => p.IsTrainee == true).Select(m => m.Id).ToListAsync();
            }
            else if (role == "Employee") {
                ids = await repositoryContext.UserIdentity.Where(p => p.IsQienEmployee == true).Select(m => m.Id).ToListAsync();
            }
            else if (role == "SoftwareDeveloper") {
                ids = await repositoryContext.UserIdentity.Where(p => p.IsSeniorDeveloper == true).Select(m => m.Id).ToListAsync();
            }

            return ids;


        }
    }
}
