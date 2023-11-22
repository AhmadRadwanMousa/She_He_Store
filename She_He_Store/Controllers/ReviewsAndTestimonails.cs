using Microsoft.AspNetCore.Mvc;
using She_He_Store.Models;
using She_He_Store.Services;

namespace She_He_Store.Controllers
{
    public class ReviewsAndTestimonails : Controller
    {

        private readonly ModelContext _context;
        private readonly IMail _mailService;
        public ReviewsAndTestimonails(ModelContext context,IMail mailService)
        {
            _context = context;
            _mailService = mailService; 
        }
        [HttpPost]
        public  IActionResult AddReview(string userName,string reviewComment, int ?rating, int productId,string categoryName)
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            string ?Email = HttpContext.Session.GetString("userName");
            if ( userId!=0)
            {
                if (rating == null)
                {
                    rating = 5;
                }
                if (userName != null && productId != null && reviewComment != null &&Email!=null && categoryName!=null)
                {
                    Review review = new Review
                    {
                        Productid = productId,
                        Reviewat = DateTime.Now,
                        Userid = userId,
                        Username = userName,
                        Reviewcomment = reviewComment,
                        Rating=rating
                    };
                    _context.Reviews.Add(review);
                    _context.SaveChanges();
                    MailData mailData = new MailData
                    {

                        EmailToName = userName,
                        EmailTo = Email,
                        EmailSubject = $"Review Added on ProductId {productId}",
                        EmailBody = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Product Review</title>\r\n</head>\r\n<body style=\"font-family: Arial, sans-serif;\">\r\n\r\n    <h1>Hello {userName},</h1>\r\n\r\n    <p>Thank you for submitting a review for Product ID: {productId}.</p>\r\n\r\n    <h2>Your Review:</h2>\r\n    <p>{reviewComment}</p>\r\n\r\n    <p>We appreciate your feedback!</p>\r\n\r\n    <p>Best regards,</p>\r\n    <p>Ahmad Mousa</p>\r\n\r\n</body>\r\n</html>"
                    };
                    _mailService.SendEmail(mailData) ;
                }
            }
            else
            {
                TempData["SignedIn"] = "false";
            }
            return RedirectToAction("ProductById", "Home", new { productId = productId,categoryName=categoryName });
        }

        [HttpPost]
        public IActionResult AddTestimonial(Testimonial testimonial)
        {
            int userId=Convert.ToInt32( HttpContext.Session.GetInt32("UserId"));
            if (userId != 0)
            {
                if (ModelState.IsValid)
                {
                     testimonial.Userid = userId;
                    _context.Testimonials.Add(testimonial);
                    _context.SaveChanges();
                }
                TempData["Successfully Added"] = "true";
            }
            else
            {
                TempData["SignedIn"] = "false";
            }
            return RedirectToAction("TestimonialPage","Home");
        }
    }
}
