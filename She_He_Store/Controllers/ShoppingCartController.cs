using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using She_He_Store.Models;

namespace She_He_Store.Controllers
{
	public class ShoppingCartController : Controller
	{
		private readonly ModelContext _context;
        public ShoppingCartController(ModelContext context)
        {
			_context = context;

		}
        public async Task<IActionResult> AddToCart(int productId, int quantity,string categoryName)
		{
			if (quantity <= 0)
			{
				quantity = 1;
			}
			int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
			var product = _context.Products.Where(p => p.Productid == productId).Include(c => c.Category).SingleOrDefault();

			if (userId != 0)
			{
				var totalPrice = ((product.Productprice - (product.Sale / 100 * product.Productprice)) * quantity);
				var userOrder = _context.Orders.Include(o=>o.Orderitems).SingleOrDefault(user => user.Userid == userId && user.Orderstatus==null);

				if (userOrder == null)
				{
					Order newOrder = new Order { Userid = userId };
					_context.Orders.Add(newOrder);
					await _context.SaveChangesAsync();
					userOrder = newOrder;
				}

				Orderitem orderitem = new Orderitem()
				{
					Orderid = userOrder.Orderid,
					Productid = productId,
					Quantity = quantity,
					Totalprice = totalPrice
				};

				_context.Orderitems.Add(orderitem);
				await _context.SaveChangesAsync();
				decimal totalOrderAmount = (decimal)userOrder.Orderitems.Sum(o => o.Totalprice);	
                userOrder.Totalamount = totalOrderAmount;
				await _context.SaveChangesAsync();
			}
			else
			{
				TempData["SignedIn"] = "false";
			}
			return RedirectToAction("ProductById","Home", new { productId = productId,categoryName=categoryName });
		}

		public IActionResult GetShoppingCart()
		 {
			int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
			if (userId != 0 && userId != null)
			{
				var orderItems = _context.Orders.Where(u => u.Userid == userId && u.Orderstatus==null).
				SelectMany(u => u.Orderitems).Include(oi=>oi.Product).ToList();	
				ViewBag.ordersCount = _context.Orders.Where(u=>u.Userid==userId && u.Orderstatus!=null).Count();	
				var wishList = _context.Wishlists.Where(w=>w.Userid==userId).Include(p=>p.Product).ToList();
				var wishListTotal = wishList.Sum(p => p.Product.Productprice-(p.Product.Sale/100*p.Product.Productprice));
				if (orderItems.Count > 0 )
				{
					ViewBag.orderItems = orderItems;
					var orderTotal = _context.Orders.SingleOrDefault(o=>o.Userid==userId &&o.Orderstatus == null).Orderitems.Sum(o=>o.Totalprice);	
					ViewBag.totalprice = orderTotal;	
					if (wishList.Count > 0)
					{
						ViewBag.wishListItems = wishList;
						ViewBag.wishListTotal = wishListTotal;
						ViewBag.emptyWishList = "false";
					}else
					{
						ViewBag.emptyWishList = "true";
					}
					return PartialView("_ShoppingCartPartial", ViewBag); 
				}
				else
				{
					ViewBag.emptyCart = "true";
					if (wishList.Count == 0)
					{
						ViewBag.emptyWishList = "true";
					}
					else
					{
						ViewBag.wishListItems = wishList;
						ViewBag.wishListTotal = wishListTotal;
						ViewBag.emptyWishList = "false";
					}
					return PartialView("_ShoppingCartPartial", ViewBag);
				}
			}
			return Json(new { notAuthirozed = true });
		}
		public async Task <IActionResult> RemoveCartItem(int ? orderItemId)
		{
			int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
			if (userId != 0 && userId != null)
			{
				var deletedItem = _context.Orderitems.Where(u => u.Orderitemid == orderItemId).SingleOrDefault();
				if (deletedItem != null)
				{
					_context.Orderitems.Remove(deletedItem);

					 var updatedOrder = _context.Orders.SingleOrDefault(o => o.Orderid == deletedItem.Orderid);
					 updatedOrder.Totalamount = updatedOrder.Totalamount - deletedItem.Totalprice;
					_context.Orders.Update(updatedOrder);
					await _context.SaveChangesAsync();
                }
			}
            return RedirectToAction("GetShoppingCart", "ShoppingCart");
        }
    
	}
}
