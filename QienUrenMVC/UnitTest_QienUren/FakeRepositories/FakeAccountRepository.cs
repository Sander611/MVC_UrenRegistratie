using QienUrenMVC.Models;
using QienUrenMVC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest_QienUren.FakeRepositories
{
    public class FakeAccountRepository : IAccountRepository
    {
        private List<AccountModel> accounts = new List<AccountModel>
        {
            new AccountModel{
            AccountId = "1",
            FirstName = "Ron",
            LastName = "Dijkstra",
            Address = "blbl",
            ZIP = "1234AB",
            City = "Amersfoort",
            IBAN = "asdsadas",
            IsActive = true,
            HashedPassword = "asdasdasd",
            ProfileImage = "sadasdasd",
            MobilePhone = "1232312",
            Email = "sadasdasd",
            IsAdmin = false,
            IsQienEmployee = false,
            IsSeniorDeveloper = false,
            IsTrainee = true,
            IsChanged = false,
            CreationDate = new DateTime(2019, 03, 03),
            DateOfBirth = new DateTime(1992, 03, 23)

            }
            };


        public async Task<AccountModel> AddNewAccount(AccountModel account)
        {
            return account;
        }

        public async Task<AccountModel> AdminUpdateAccount(AccountModel account, string uniqueFilename)
        {
            var userpersonalia = new UserPersonaliaModel();
            return account;
        }

        public Task<UserPersonaliaModel> ComparePersonaliaChanges(string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountModel> GetAccountByFormId(int formId)
        {
            return accounts[0];
        }

        public Task<List<string>> GetAccountIdsByRole(string role)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AccountModel>> GetAllAccounts(string searchString)
        {
            return accounts;      
        }

        public async Task<List<AccountModel>> GetAllAccountsWithoutString()
        {
            var allAccounts = new List<AccountModel>();
            return allAccounts;
        }

        public Task<List<AccountModel>> GetAllQienMedewerkers()
        {
            throw new NotImplementedException();
        }

        public Task<List<AccountModel>> GetAllSeniors()
        {
            throw new NotImplementedException();
        }

        public Task<List<AccountModel>> GetAllTrainees()
        {
            throw new NotImplementedException();
        }

        public Task<List<AccountModel>> GetChangedAccounts()
        {
            throw new NotImplementedException();
        }

        public async Task<AccountModel> GetOneAccount(string accountId)
        {
            return accounts[0];
        }

        public Task<List<AccountModel>> getPersonaliaFromAccount(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<AccountModel> ModifyAccountActivity(string accountId, bool IsActive)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAccount(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllFormPerAccount(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task RevertAccountPersonalia(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task SetAccountChanged(string accountId, bool isChanged)
        {
            throw new NotImplementedException();
        }

        public Task setUserActive(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountModel> UpdateAccount(AccountModel account, string UniqueFilename)
        {
            return accounts[0];


        }
    }
}
