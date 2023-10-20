using CapstoneProject.Models.Account_Models;

namespace CapstoneProject.Models
{
    public class ConfrimationRepository
    {
        private static List<EmailConfirmationInformation> _confrimations = new List<EmailConfirmationInformation>();

        public static void addConfirmation(EmailConfirmationInformation ci)
        {
            _confrimations.Add(ci);
        }

        public static IEnumerable<EmailConfirmationInformation> GetConfirmations => _confrimations;
    }
}
