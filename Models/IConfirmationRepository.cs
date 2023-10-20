using CapstoneProject.Models.Account_Models;

namespace CapstoneProject.Models
{
    public interface IConfirmationRepository
    {
        IQueryable<EmailConfirmationInformation> GetAllConfirmations { get; }
        public void AddConfirmation(EmailConfirmationInformation confirmation);
        
        public void DeleteConfirmation(int id);
    }
}
