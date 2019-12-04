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
        string RemoveAccount(string accountId);
        Task<AccountModel> UpdateAccount(AccountModel account, string UniqueFilename);
        Task<List<AccountModel>> getPersonaliaFromAccount(string accountId);
        Task<List<string>> GetAccountIdsByRole(string role);
        Task<AccountModel> GetAccountByFormId(int formId);
        Task<List<AccountModel>> GetChangedAccounts();

    }
}