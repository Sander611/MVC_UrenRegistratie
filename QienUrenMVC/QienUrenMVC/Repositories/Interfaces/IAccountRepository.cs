using System.Collections.Generic;
using System.Threading.Tasks;
using QienUrenMVC.Models;


namespace QienUrenMVC.Repositories
{
    public interface IAccountRepository
    {
        Task<AccountModel> AddNewAccount(AccountModel account);
        Task<List<AccountModel>> GetAllAccounts(string searchString);
        Task<AccountModel> GetOneAccount(string accountId);
        Task<AccountModel> ModifyAccountActivity(string accountId, bool IsActive);
        Task RemoveAccount(string accountId);
        Task RemoveAllFormPerAccount(string accountId);
        Task<AccountModel> UpdateAccount(AccountModel account, string UniqueFilename);
        Task<List<AccountModel>> getPersonaliaFromAccount(string accountId);
        Task<List<string>> GetAccountIdsByRole(string role);
        Task<AccountModel> GetAccountByFormId(int formId);
        Task<List<AccountModel>> GetChangedAccounts();
        Task<List<AccountModel>> GetAllAccountsWithoutString();
        Task<UserPersonaliaModel> ComparePersonaliaChanges(string accountId);
        Task<List<AccountModel>> GetAllSeniors();
        Task<List<AccountModel>> GetAllQienMedewerkers();
        Task<List<AccountModel>> GetAllTrainees();
        Task SetAccountChanged(string accountId, bool isChanged);

        Task RevertAccountPersonalia(string accountId);
        Task setUserActive(string id);
    }
}