using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;

namespace Hello_World
{
    [TestClass]
    public class SongInfoReviewTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var sw = new StringWriter())
            {
                Review review = new Review();
                review.Review_ID = 999;
                review.Review_Star = 1;
                review.Review_Entry = "Test";
                review.User_ID_FK = 12345678;
                review.Song_ID_FK = 1;

                ReviewDBManager.DBCreateReview(review);
                Review reviewTest = ReviewDBManager.GetReviewById(999);


                Assert.AreEqual(review.Review_ID, reviewTest.Review_ID);
                
            }
        }
    }
}