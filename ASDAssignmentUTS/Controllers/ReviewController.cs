﻿using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASDAssignmentUTS.Controllers
{
    public class ReviewController : Controller
    {
        public IActionResult DisplayReviews(int? songId)
        {
            if (songId.HasValue)
            {
                // Get the song information
                Song song = SongDBManager.GetSongById(songId.Value);

                // Get the reviews for the specified song ID
                List<Review> reviews = ReviewDBManager.GetReviewsBySongId(songId.Value);

                // Pass both the song and reviews to the view
                ViewData["Song"] = song;
                ViewData["Reviews"] = reviews;

                return View(song);
            }
            else
            {
                return View("Error");
            }
        }
        [HttpPost]
        public IActionResult AddReview(int songId, int stars, string description)
        {
            // Create a new Review object with the provided data
            Review newReview = new Review()
            {
                Review_Star = stars,
                Review_Entry = description,
                User_ID_FK = 12345678, // Set the appropriate user ID
                Song_ID_FK = songId
            };

            // Call a method to add the new review to the database
            ReviewDBManager.DBCreateReview(newReview);

            // Redirect back to the review page
            return RedirectToAction("DisplayReviews", new { songId });
        }
    }



}
