namespace CapstoneProject.Models.Account_Models
{
    public class ReviewRepository
    {
        private static List<ReviewInformation> _reviews = new List<ReviewInformation>();

        public static void addAccount(ReviewInformation ri)
        {
            _reviews.Add(ri);
        }

        public static IEnumerable<ReviewInformation> GetReviews => _reviews;
    }
}
