namespace CapstoneProject.Models.Account_Models
{
    public interface IReviewRepository
    {
        IQueryable<ReviewInformation> GetAllReviews { get; }
        public void AddReview(ReviewInformation review);
    }
}
