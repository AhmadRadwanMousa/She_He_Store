using MailKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using She_He_Store.Helpers;
using She_He_Store.Models;
using She_He_Store.Services;
using static System.Net.WebRequestMethods;

namespace She_He_Store.Controllers
{
    public class LoginAndRegisterController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMail _mailService;
        public LoginAndRegisterController(ModelContext context, IWebHostEnvironment webHostEnvironment, IMail mailService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _mailService = mailService;
        }
        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            if (userName != null && password != null)
            {
                Userlogin? auth = _context.Userlogins.Where(u => u.Username == userName && u.Password == password).Include(u => u.User).SingleOrDefault();
                if (auth != null)
                {
                    HttpContext.Session.SetInt32("UserId", Convert.ToInt32(auth.Userid));
                    HttpContext.Session.SetString("Fname", auth.User.Fname);
                    HttpContext.Session.SetString("userName",auth.Username);
                    HttpContext.Session.SetString("ProfileImage",auth.User.Profilepicture);
                    switch (auth.Roleid)
                    {
                        case 0:
                            return RedirectToAction("Dashboard", "Admin");
                        case 1: return RedirectToAction("HomePage", "Home", auth.Userid);
                        default: break;
                    }
                }
                else
                {
                    TempData["WrongCredentials"] = "true";
                }

            }
            return RedirectToAction("LoginAndRegisterPage");
        }

        [HttpGet]
        public IActionResult LoginAndRegisterPage()
        {
            List<SelectListItem> Gender = new List<SelectListItem>
            {
                new SelectListItem {Text="Male",Value="Male"},
                new SelectListItem { Text="Female",Value="Female"}
            };
            ViewBag.Gender = Gender;
            if (TempData.Peek("ExistUser")!=null && TempData["ExistUser"].ToString() == "true")
            {
                ViewBag.ExistUser = "true";
            }
            else if (TempData.Peek("WrongCredentials")!=null && TempData["WrongCredentials"].ToString() == "true")
            {
                ViewBag.WrongCredentials = "true";
            }
            else if (TempData.Peek("NotExist")!=null && TempData["NotExist"].ToString() == "true")
            {
                ViewBag.NotExist = "true";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user, string userName, string password)
        {
            if (ModelState.IsValid)
            {
                if (userName != null && password != null)
                {
                    var isExist = _context.Userlogins.SingleOrDefault(user=>user.Username==userName);
                    if (isExist !=null) {
                        TempData["ExistUser"] = "true";
                        return RedirectToAction("LoginAndRegisterPage");
                    }
                    else if(user.ImageFile != null)
                    {
						string imagePath = await ImageUploader.UploadeImage(_webHostEnvironment, user.ImageFile);
						user.Profilepicture = imagePath;
						user.Createdat = DateTime.Now;
						_context.Users.Add(user);
						await _context.SaveChangesAsync();
						Userlogin userLogin = new Userlogin()
						{
							Userid = user.Userid,
							Username = userName,
							Password = password,
							Roleid = 1,
						};
						_context.Userlogins.Add(userLogin);
						await _context.SaveChangesAsync();
						HttpContext.Session.SetInt32("UserId", Convert.ToInt32(user.Userid));
						HttpContext.Session.SetString("Fname", user.Fname);
                        HttpContext.Session.SetString("userName", userName);
                        HttpContext.Session.SetString("ProfileImage", user.Profilepicture);
						return RedirectToAction("HomePage", "Home");
					}
                }

            }
            return RedirectToAction("LoginAndRegister");
         
        }
        [HttpPost]
        public IActionResult ForgetPassword(string? userName)
        {
            var user = _context.Userlogins.Where(e => e.Username == userName).Include(u => u.User).SingleOrDefault();
            if (user != null)
            {
                Random random = new Random();
                int OTP = random.Next(1000, 9999);
                string EmailBody = $@"<p style='color:red;'>Hello there {userName} and welcome!</p><p>Your OTP is: {OTP.ToString("D4")}</p>";
                _mailService.SendEmail(new MailData
                {
                    EmailSubject = "OTP Code",
                    EmailTo = userName,
                    EmailToName = user.User.Fname,
                    EmailBody = EmailBody,
                });
                ViewBag.OTP = OTP;
                TempData["userName"] = userName;
                return PartialView("_OTPPartial",ViewBag.OTP );
            }
            TempData["NotExist"] = "true";
           return RedirectToAction("LoginAndRegisterPage");
		}
        [HttpGet]
        public async Task<IActionResult> RenderNewPasswordPartial()
        {
            return PartialView("_NewPasswordPartial");
        }
        [HttpPost]
        public IActionResult HandleNewPassword(string password)
        {
            string userName = TempData["userName"].ToString();
            if (!string.IsNullOrEmpty(userName))
            {
                var user = _context.Userlogins.Where(user => user.Username == userName).SingleOrDefault();
                if (password != null && user != null)
                {
                    user.Password = password;
                    _context.Userlogins.Update(user);
                    _context.SaveChanges();
                }
                ViewBag.Updated = "true";
                return View("LoginAndRegisterPage");
            }
            return Content("Something went wrong!");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Fname");
            return RedirectToAction("LoginAndRegisterPage");
        }

    }
}
