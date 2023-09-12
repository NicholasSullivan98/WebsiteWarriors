namespace CapstoneProject.Models.Account_Models
{
    public interface IReviewRepository
    {
        IQueryable<ReviewInformation> GetAllReviews { get; }
        ReviewInformation Get3Reviews();
        public void AddReview(ReviewInformation review);
    }
}
