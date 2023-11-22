using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using She_He_Store.Models;

namespace She_He_Store.Controllers
{
	public class AdminController : Controller
	{
		private readonly ModelContext _context;
		public AdminController(ModelContext context)
		{
			_context = context;
		}
		public IActionResult Dashboard()
		{
			ViewBag.numberOfUsers = _context.Users.Count();
			ViewBag.numberOfOrders = _context.Orders.Count();
			ViewBag.totalEarned = _context.Orders.Sum(o => o.Totalamount);
			ViewBag.numberOfProducts = _context.Products.Count();
			ViewBag.numberOfCategories = _context.Categories.Count();
			ViewBag.numberOfTestimonails = _context.Testimonials.Count();
			ViewBag.numberOfReviews = _context.Reviews.Count();
			ViewBag.numberOfCards = _context.Cards.Count();
			ViewBag.numberOfLaptops = _context.Products.Count(p => p.Category.Categoryname == "Laptops");
			ViewBag.numberOfPhones = _context.Products.Count(p => p.Category.Categoryname == "Smartphones");
			ViewBag.numberOfAccsessories = _context.Products.Count(p => p.Category.Categoryname == "Accsessories");
			ViewBag.numberOfGames = _context.Products.Count(p => p.Category.Categoryname == "Games");
			ViewBag.orders = _context.Orders.Where(o => o.Orderstatus != null).Include(o => o.User).ToList();


			return View();
		}
		public IActionResult Testimonails()
		{
			var testimonails = _context.Testimonials.ToList();
			return View(testimonails);
		}
		public IActionResult AcceptTestimonail(int testId)
		{
			var testimonail = _context.Testimonials.Where(t => t.Testimonialid == testId).SingleOrDefault();
			if (testimonail != null)
			{
				testimonail.Testimonialstate = "Accepted";
				_context.Testimonials.Update(testimonail);
				_context.SaveChanges();
			}
			return RedirectToAction("Testimonails");
		}
		public IActionResult RejectTestimonail(int testId)
		{
			var testimonail = _context.Testimonials.Where(t => t.Testimonialid == testId).SingleOrDefault();
			if (testimonail != null)
			{
				_context.Testimonials.Remove(testimonail);
				_context.SaveChanges();
			}

			return RedirectToAction("Testimonails");
		}
		public IActionResult UserOrders()
		{
			var orders = _context.Orders.Where(o => o.Orderstatus != null).ToList();
			if (orders != null)
			{
				return View(orders);
			}
			ViewBag.emptyOrders = "true";
			return View(orders);
		}
		public IActionResult ChangeOrderState(int orderId, string orderState)
		{
			var order = _context.Orders.SingleOrDefault(o => o.Orderid == orderId);
			if (order != null)
			{
				if (orderState == "Rejected")
				{
					_context.Orders.Remove(order);
					_context.SaveChanges();
					return RedirectToAction("UserOrders", "Admin");
				}
				order.Orderstatus = orderState;
				_context.Orders.Update(order);
				_context.SaveChanges();
			}
			return RedirectToAction("UserOrders", "Admin");
		}
		public IActionResult NotAuthorized()
		{
			return View();
		}
		public IActionResult Report()
		{
			var productsWithCategories = _context.Products.Include(p => p.Category).ToList();
			var userOrders = _context.Orders.Where(o => o.Orderstatus != null).Include(o => o.Payment).Include(u => u.User)
				.Select(o => new OrderViewModel
				{
					Order = o,
					OrderItems = o.Orderitems.Select(oi => new OrderItemWithProduct
					{
						OrderItem = oi,
						Product = oi.Product
					}).ToList()
				}).ToList();
			var tuple = Tuple.Create<IEnumerable<Product>, IEnumerable<OrderViewModel>>(productsWithCategories, userOrders);
			return View(tuple);
		}
		public IActionResult GetChartData()
		{
			var laptopsCount = _context.Products.Count(p => p.Category.Categoryname == "Laptops");
			var SmartPhonesCount = _context.Products.Count(p => p.Category.Categoryname == "Smartphones");
			var AccsessoriesCount = _context.Products.Count(p => p.Category.Categoryname == "Accsessories");
			var GamesCount = _context.Products.Count(p => p.Category.Categoryname == "Games");
			var chartData = new List<ChartData> {
			new ChartData{ Label="Laptops",Value=laptopsCount,Color="#ffab00"},
			new ChartData{Label="SmartPhones",Value=SmartPhonesCount,Color="#00d25b"},
			new ChartData{Label="Accsessories",Value=AccsessoriesCount,Color="#111111"},
			new ChartData{Label="Games",Value=GamesCount,Color="#8E97FC"},
			};
			return Json(chartData);
		}
		public IActionResult Users()
		{
			var users = _context.Users.ToList();
			return View(users);
		}
		public IActionResult Roles()
		{
			var roles = _context.Roles.Include(u => u.Userlogins).ToList();
			return View(roles);
		}
		public IActionResult Payments()
		{
			var Payments = _context.Payments.ToList();
			return View(Payments);
		}


		[HttpGet]
		public IActionResult AddRole()
		{
			return View();
		}
		[HttpPost]
		public IActionResult AddRole(Role role)
		{
			if (role.Rolename!=null)
			{
				  
				_context.Roles.Add(role);
				_context.SaveChanges();
                return RedirectToAction("Roles");
            }
			return View("AddRole");
		}


	

	}
}

