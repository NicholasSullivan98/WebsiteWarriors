using CapstoneProject.Models.Account_Models;

namespace CapstoneProject.Models
{
    public class ManageAccountPageModel
    {
        public AccountInformation Accounts { get; set; }
        public IEnumerable<StudentInformation> Students { get; set; }
    }
}
