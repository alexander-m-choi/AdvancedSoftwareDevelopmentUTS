using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASDAssignmentUTS.Controllers
{
    public class ReviewController : Controller
    {

        public IActionResult DisplayReviews(int? songId)
        {
            List<Review> reviews;

            if (songId.HasValue)
            {
                // Filter reviews for one song ID
                reviews = ReviewDBManager.GetReviewsBySongId(songId.Value);
            }
            else
            {
                // Get all reviews if no song ID is provided
                reviews = ReviewDBManager.GetReviews();
            }

            return View("DisplayReviews", reviews);
        }

        [HttpPost]
        public ActionResult CreateReview(int songId, string starRating, string reviewText)
        {
            // Variables to hold star and text.
            int reviewStar = int.Parse(starRating);
            string reviewEntry = reviewText;

            // Create a new Review
            Review review = new Review()
            {
                Review_Star = reviewStar,
                Review_Entry = reviewEntry,
                User_ID_FK = 12345678,
                Song_ID_FK = songId // Use the provided songId
            };

            // Creating a new review in the database
            ReviewDBManager.DBCreateReview(review);

            List<Review> reviews;
            reviews = ReviewDBManager.GetReviewsBySongId(songId);
            // Load page again
            return View("DisplayReviews", reviews);
        }
    } 

}
