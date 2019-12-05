﻿using Microsoft.AspNetCore.Identity;
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

        public async Task<List<AccountModel>> GetChangedAccounts()
        {
            var allAccounts = new List<AccountModel>();
            List<UserIdentity> changedUsers = await repositoryContext.UserIdentity.Where(p => p.IsChanged == true).ToListAsync();

            foreach (var account in changedUsers)

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
                    IsTrainee = account.IsTrainee,
                    IsChanged = account.IsChanged
                });

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
            };

            var result = await _userManager.CreateAsync(accountEntity, account.HashedPassword);
            await _userManager.AddToRoleAsync(accountEntity, "Employee");

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
                HashedPassword = account.PasswordHash,
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

        public async Task<AccountModel> UpdateAccount(AccountModel account, string uniqueFilename)
        {
            UserIdentity entity = repositoryContext.UserIdentity.Single(p => p.Id == account.AccountId);

            UserPersonalia personalia = new UserPersonalia();
            personalia.AccountId = entity.Id;
            personalia.FirstName = entity.FirstName;
            personalia.LastName = entity.LastName;
            personalia.Address = entity.Address;
            personalia.City = entity.City;
            personalia.CreationDate = entity.CreationDate;
            personalia.DateOfBirth = entity.DateOfBirth;
            personalia.IBAN = entity.IBAN;
            personalia.IsActive = entity.IsActive;
            personalia.IsAdmin = entity.IsAdmin;
            personalia.IsQienEmployee = entity.IsQienEmployee;
            personalia.IsSeniorDeveloper = entity.IsSeniorDeveloper;
            personalia.IsTrainee = entity.IsTrainee;
            personalia.IsChanged = entity.IsChanged;
            //Email and phonenumber are missing..

            entity.FirstName = account.FirstName;
            entity.LastName = account.LastName;
            entity.Address = account.Address;
            entity.City = account.City;
            entity.CreationDate = account.CreationDate;
            entity.DateOfBirth = account.DateOfBirth;
            entity.Email = account.Email;
            entity.IBAN = account.IBAN;
            entity.ProfileImage = uniqueFilename;
            entity.ZIP = account.ZIP;
            entity.IsActive = account.IsActive;
            entity.IsAdmin = account.IsAdmin;
            entity.IsQienEmployee = account.IsQienEmployee;
            entity.IsSeniorDeveloper = account.IsSeniorDeveloper;
            entity.PhoneNumber = account.MobilePhone;
            entity.IsTrainee = account.IsTrainee;
            entity.IsChanged = account.IsChanged;

            
            //saving User info in Userpersonalia table
            repositoryContext.UserPersonalia.Add(personalia);

            
            await repositoryContext.SaveChangesAsync();
            
            return account;
        }

        public async Task <UserPersonaliaModel> ComparePersonaliaChanges(string accountId)
        {
            //var PreviousPersonalia = await repositoryContext.UserIdentity.Where(p => p.Id == accountId).ToListAsync();
            
            
            UserPersonalia oldPersonalia = await repositoryContext.UserPersonalia.FirstAsync(p => p.AccountId == accountId);
            UserIdentity newPersonalia = await repositoryContext.UserIdentity.FirstAsync(p => p.Id == accountId);
           
            UserPersonaliaModel userPersonaliaModel = new UserPersonaliaModel();

            userPersonaliaModel.previousPersonalia = new UserPersonaliaModel();
            userPersonaliaModel.newPersonalia = new UserPersonaliaModel();

            userPersonaliaModel.previousPersonalia.FirstName = oldPersonalia.FirstName;
            userPersonaliaModel.previousPersonalia.LastName = oldPersonalia.LastName;
            userPersonaliaModel.previousPersonalia.AccountId = oldPersonalia.AccountId;
            userPersonaliaModel.previousPersonalia.Address = oldPersonalia.Address;
            userPersonaliaModel.previousPersonalia.City = oldPersonalia.City;
            userPersonaliaModel.previousPersonalia.CreationDate = oldPersonalia.CreationDate;
            userPersonaliaModel.previousPersonalia.DateOfBirth = oldPersonalia.DateOfBirth;
            userPersonaliaModel.previousPersonalia.CreationDate = oldPersonalia.CreationDate;
            userPersonaliaModel.previousPersonalia.ZIP = oldPersonalia.ZIP;
            userPersonaliaModel.previousPersonalia.IBAN = oldPersonalia.IBAN;
            userPersonaliaModel.previousPersonalia.ProfileImage = oldPersonalia.ProfileImage;
            userPersonaliaModel.previousPersonalia.IsTrainee = oldPersonalia.IsTrainee;
            userPersonaliaModel.previousPersonalia.IsQienEmployee = oldPersonalia.IsQienEmployee;
            userPersonaliaModel.previousPersonalia.IsSeniorDeveloper = oldPersonalia.IsSeniorDeveloper;
            userPersonaliaModel.previousPersonalia.IsChanged = oldPersonalia.IsChanged;


            //new personalia
            userPersonaliaModel.newPersonalia.FirstName = newPersonalia.FirstName;
            userPersonaliaModel.newPersonalia.LastName = newPersonalia.LastName;
            userPersonaliaModel.newPersonalia.AccountId = newPersonalia.Id;
            userPersonaliaModel.newPersonalia.Address = newPersonalia.Address;
            userPersonaliaModel.newPersonalia.City = newPersonalia.City;
            userPersonaliaModel.newPersonalia.CreationDate = newPersonalia.CreationDate;
            userPersonaliaModel.newPersonalia.DateOfBirth = newPersonalia.DateOfBirth;
            userPersonaliaModel.newPersonalia.CreationDate = newPersonalia.CreationDate;
            userPersonaliaModel.newPersonalia.ZIP = newPersonalia.ZIP;
            userPersonaliaModel.newPersonalia.IBAN = newPersonalia.IBAN;
            userPersonaliaModel.newPersonalia.ProfileImage = newPersonalia.ProfileImage;
            userPersonaliaModel.newPersonalia.IsTrainee = newPersonalia.IsTrainee;
            userPersonaliaModel.newPersonalia.IsQienEmployee = newPersonalia.IsQienEmployee;
            userPersonaliaModel.newPersonalia.IsSeniorDeveloper = newPersonalia.IsSeniorDeveloper;
            userPersonaliaModel.newPersonalia.IsChanged = newPersonalia.IsChanged;
            return userPersonaliaModel;
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
        public async Task<AccountModel> GetAccountByFormId(int formId)
        {
            HoursForm hoursForm = await repositoryContext.HoursForms.SingleAsync(a => a.FormId == formId);

            UserIdentity account = await repositoryContext.UserIdentity.SingleAsync(p => p.Id == hoursForm.AccountId);

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

        public async Task SetAccountChanged(string accountId,bool isChanged)
        {
            UserIdentity account = await repositoryContext.UserIdentity.SingleOrDefaultAsync(p => p.Id == accountId);

            account.IsChanged = isChanged;

            repositoryContext.Update(account);
            await repositoryContext.SaveChangesAsync();
        }

        public async Task RevertAccountPersonalia(string accountId)
        {
            UserIdentity account = await repositoryContext.UserIdentity.SingleOrDefaultAsync(p => p.Id == accountId);


            UserPersonalia personalia = await repositoryContext.UserPersonalia.SingleOrDefaultAsync(p => p.AccountId == accountId);

            account.FirstName = personalia.FirstName;
            account.LastName = personalia.LastName;
            account.ZIP = personalia.ZIP;
            account.Address = personalia.Address;
            account.City = personalia.City;
            account.IsChanged = false;

            
            repositoryContext.Update(account);
            await repositoryContext.SaveChangesAsync();
        }
    }
}
