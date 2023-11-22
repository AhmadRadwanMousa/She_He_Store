using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using She_He_Store.Helpers;
using She_He_Store.Models;

namespace She_He_Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ModelContext modelContext,IWebHostEnvironment webHostEnvironment)
        {
            _context = modelContext;
            _webHostEnvironment = webHostEnvironment;
        }
        [Route("/")]
        public IActionResult HomePage()
        {
            var categories = _context.Categories.ToList();
            var productsCategories = _context.Products.Include(p => p.Category).ToList();
            var acceptedTestimonails = _context.Testimonials.Where(t => t.Testimonialstate == "Accepted").Include(u => u.User).ToList();
            ViewBag.Testimonails = acceptedTestimonails;
            if (TempData["categoryName"] != null)
            {
                var productCategories = _context.Products.Include(p => p.Category)
                .Where(p => p.Category != null && p.Category.Categoryname == TempData["categoryName"])
                .ToList();
                if (!productCategories.Any())
                {
                    ViewBag.empty = true;
                }
                else
                {
                    ViewBag.ItemsByCategory = productCategories;
                }

            }
            else
            {
                ViewBag.ItemsByCategory = productsCategories;
            }

            return View(categories);


        }
        [HttpGet]
        public IActionResult FindByCategory(string? categoryName)
        {
            if (categoryName != null)
            {
                TempData["categoryName"] = categoryName;
            }
            return RedirectToAction("HomePage");
        }
        public IActionResult ProductById(int? productId, string categoryName)
        {

            if (productId != null)
            {
                var product = _context.Products.Where(p => p.Productid == productId).Include(c => c.Category).SingleOrDefault();

                var proudctCategories = _context.Products.Where(p => p.Category.Categoryname == categoryName).Include(c => c.Category).ToList();
                var reviewsPerProduct = _context.Reviews.Where(p => p.Productid == productId).ToList();

                var productCategoriesTuple = Tuple.Create<Product, IEnumerable<Product>, IEnumerable<Review>>(product, proudctCategories, reviewsPerProduct);

                if (TempData.Peek("SignedIn") != null && TempData["SignedIn"].ToString() == "false")
                {
                    ViewBag.SignIn = "false";
                }
                return View(productCategoriesTuple);
            }

            return View("HomePage");
        }

        public IActionResult ProductsByCategory(string categoryName, FilterProductsModel filterProduct)
        {
            var categories = _context.Categories.Include(p => p.Products).ToList();
            var products = _context.Products.Include(p => p.Category).ToList();

            if (categoryName != null)
            {
                products = products.Where(p => p.Category.Categoryname == categoryName).ToList();
            }
            else 
            if (filterProduct.Category != null)
            {

                products = products.Where(p => p.Category.Categoryname == filterProduct.Category).ToList();
                if (filterProduct.ProductName != null)
                {
                    products = products.Where(p => p.Productname.IndexOf(filterProduct.ProductName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }
                if (filterProduct.MaxPrice!=0 && filterProduct.MaxPrice != 0)
                {
					products = products.Where(p => p.Productprice >= filterProduct.MinPrice && p.Productprice <= filterProduct.MaxPrice).ToList();
				}

				if (filterProduct.SaleSort != null)
                {

                    int saleSort = int.Parse(filterProduct.SaleSort);
                    products = products.Where(p => p.Sale <= saleSort).ToList();
                }


                if (filterProduct.PriceSort == "DES")
                {
                    products = products.OrderByDescending(p => p.Productprice).ToList();
                }
                else
                {
                    products = products.OrderBy(p => p.Productprice).ToList();
                }

            }

            if (products.Count() == 0)
            {
                ViewBag.EmptyProducts = "true";
            }
            var productCategoriesTuple = Tuple.Create<IEnumerable<Category>, IEnumerable<Product>>(categories, products);
            return View(productCategoriesTuple);
        }

        public IActionResult TestimonialPage()
        {
            if (TempData.Peek("SignedIn") != null && TempData["SignedIn"].ToString() == "false")
            {
                ViewBag.SignIn = "false";
            }
            else if (TempData.Peek("Successfully Added") != null && TempData["Successfully Added"].ToString() == "true")
            {
                ViewBag.AddedTestimonail = "true";
            }

            return View();
        }
        public IActionResult CheckoutPage()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            if (userId != 0)
            {
                var order = _context.Orders.SingleOrDefault(o => o.Userid == userId && o.Orderstatus == null);

                if (order != null)
                {
                    var orderItems = _context.Orders.Where(o => o.Userid == userId && o.Orderstatus == null).SelectMany(u => u.Orderitems).Include(oi => oi.Product).ToList();
                    OrderAndCardDetails orderAndCardDetails = new OrderAndCardDetails
                    {
                        orderItems = orderItems,
                        card = null
                    };
                    ViewBag.total = order.Orderitems.Sum(o => o.Totalprice);
                    ViewBag.orderId = order.Orderid;
                    if (TempData.Peek("NotSuccsesfullPayment") != null && TempData["NotSuccsesfullPayment"].ToString() == "true")
                    {
                        ViewBag.NotSuccsesfullPayment = "true";
                    }
                    return View(orderAndCardDetails);
                }
            }
            return RedirectToAction("HomePage");
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SearchProduct(string category, int minPrice, int maxPrice, string? productName, string saleSort, string priceSort)
        {
            FilterProductsModel filterProducts = new FilterProductsModel
            {
                Category = category,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SaleSort = saleSort,
                PriceSort = priceSort,
                ProductName = productName,
            };
            return RedirectToAction("ProductsByCategory", filterProducts);
        }
        public IActionResult Orders()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            if (userId != 0)
            {
                var userOrders = _context.Orders.Where(u => u.Userid == userId && u.Orderstatus != null).Include(o => o.Payment)
                .Select(o => new OrderViewModel
                 {
                        Order = o,
                        OrderItems = o.Orderitems.Select(oi => new OrderItemWithProduct
                        {
                           OrderItem = oi,
                         Product = oi.Product
                         }).ToList()}).ToList();

                if (userOrders.Count() == 0)
                {
                    return RedirectToAction("HomePage", "Home");
                }
                if ( TempData.Peek("PaymentSuccessfully") != null && TempData["PaymentSuccessfully"].ToString() =="true" )
                {
                    ViewBag.orderPlaced = "true";
                }
                else
                {
                    ViewBag.orderPlaced = "false";
                }
                return View(userOrders);
            }
            return RedirectToAction("HomePage", "Home");
        }
        public IActionResult EditProfile()
        {
              int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
              if (userId!=0)
            {
                var user = _context.Users.Where(u=>u.Userid==userId).SingleOrDefault();
                var password = _context.Userlogins.SingleOrDefault(u=>u.Userid==userId).Password;

                ViewBag.password=password;
                if (TempData.Peek("ChangedSuccessfully")!=null && TempData["ChangedSuccessfully"].ToString() == "true")
                {
                    ViewBag.Updated="true";
                }
                return View(user);
            }
            return View("LoginAndRegisterPage","LoginAndRegister");
        }
        [HttpPost]
        public async Task< IActionResult> EditProfile(User user,string Password)
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            if (userId != 0)
            {
                if (ModelState.IsValid)
                {
                    if (user.ImageFile != null)
                    {
                        string? imagefile = await ImageUploader.UploadeImage(_webHostEnvironment, user.ImageFile);
                        if (imagefile != null)
                        {
                            user.Profilepicture = imagefile;
                            user.Updatedat = DateTime.Now;
                            _context.Users.Update(user);
                            _context.SaveChanges(); 
                        }
                        var userLogin = _context.Userlogins.SingleOrDefault(u => u.Userid == userId);
                        userLogin.Password = Password;
                        _context.Userlogins.Update(userLogin);
                        _context.SaveChanges();
                        TempData["ChangedSuccessfully"] = "true";
                        return RedirectToAction("EditProfile");
                    }
                }
            }
            return View("LoginAndRegisterPage", "LoginAndRegister");
        }
        public IActionResult ContactUs()
        {
            return View();
        }
    }
}
