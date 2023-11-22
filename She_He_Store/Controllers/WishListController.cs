using Microsoft.AspNetCore.Mvc;
using She_He_Store.Models;

namespace She_He_Store.Controllers
{
	public class WishListController : Controller
	{
		private readonly ModelContext _context;
        public WishListController(ModelContext context)
        {
			_context = context;
        }
        public IActionResult AddToWishList(int productId,string categoryName)
		{
			int userId =Convert.ToInt32( HttpContext.Session.GetInt32("UserId"));
			if (userId !=0) {
				if (productId != 0 && categoryName!=null)
				{
					Wishlist newWishlist = new Wishlist
					{
						Userid = userId,
						Productid = productId,
						Addedat = DateTime.Now,
					};
					_context.Wishlists.Add(newWishlist);
					_context.SaveChanges();
				}
			}
			else
			{
				TempData["SignedIn"] = "false";
			}
			return RedirectToAction("ProductById", "Home", new { productId = productId, categoryName = categoryName });
		}
		public IActionResult RemoveWishList(int ?wishListId)
		{
			var deletedWishList = _context.Wishlists.SingleOrDefault(w=>w.Wishlistid==wishListId);
			if (deletedWishList != null) {
				_context.Wishlists.Remove(deletedWishList);
				_context.SaveChanges();
			}
			return RedirectToAction("GetShoppingCart", "ShoppingCart");
		}
	}
}
