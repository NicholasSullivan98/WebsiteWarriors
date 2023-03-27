using Microsoft.AspNetCore.Mvc;

namespace CapstoneProject.Models.Account_Models
{
    public class AccountRepository
    {
        private static List<AccountInformation> _accounts = new List<AccountInformation>();

        public static void addAccount(AccountInformation ai)
        {
            _accounts.Add(ai);
        }

        public static IEnumerable<AccountInformation> GetAccounts => _accounts;
    }
}
