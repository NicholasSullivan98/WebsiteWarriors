namespace CapstoneProject.Models.Account_Models
{
    public interface IAccountRepository
    {
        IQueryable<AccountInformation> GetAllAccounts { get; }
        public void AddUser(AccountInformation account);
    }
}
