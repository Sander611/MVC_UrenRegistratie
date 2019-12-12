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
        public Task<AccountModel> AddNewAccount(AccountModel account)
        {
            throw new NotImplementedException();
        }

        public Task<AccountModel> AdminUpdateAccount(AccountModel account, string uniqueFilename)
        {
            throw new NotImplementedException();
        }

        public Task<UserPersonaliaModel> ComparePersonaliaChanges(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<AccountModel> GetAccountByFormId(int formId)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetAccountIdsByRole(string role)
        {
            throw new NotImplementedException();
        }

        public Task<List<AccountModel>> GetAllAccounts(string searchString)
        {
            throw new NotImplementedException();
        }

        public Task<List<AccountModel>> GetAllAccountsWithoutString()
        {
            throw new NotImplementedException();
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

        public Task<AccountModel> GetOneAccount(string accountId)
        {
            throw new NotImplementedException();
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

        public Task<AccountModel> UpdateAccount(AccountModel account, string UniqueFilename)
        {
            throw new NotImplementedException();
        }
    }
}
