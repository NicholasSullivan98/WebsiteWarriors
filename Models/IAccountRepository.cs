namespace CapstoneProject.Models.Account_Models
{
    public interface IAccountRepository
    {
        IQueryable<AccountInformation> GetAllAccounts { get; }
        public void AddUser(AccountInformation account);
        public AccountInformation GetLoggedInAccountInfo(int id);
        public AccountInformation UpdateUser(AccountInformation ai, int id);
    }
}
