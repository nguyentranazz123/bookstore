using Code2.Data;
using Code2.Infrastructure;
using Code2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Code2.Controllers
{
	[Authorize(Roles = "Customer")]
	public class CartController : Controller
	{
		public Cart? Cart { get; set; }
		private readonly ApplicationDbContext _context;

		public CartController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult ViewCart()
		{
			Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
			return View("Cart", Cart);
		}

		public IActionResult AddToCart(int id)
		{
			Book? book = _context.Book?.FirstOrDefault(b => b.Id == id);
			if (book != null)
			{
				Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
				Cart.AddItem(book, 1);
				HttpContext.Session.SetJson("cart", Cart);
			}
			return View("Cart", Cart);
		}

		public IActionResult UpdateCart(int id)
		{
			Book? book = _context.Book?.FirstOrDefault(b => b.Id == id);
			if (book != null)
			{
				Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
				Cart.DecreaseItem(book, 1);
				HttpContext.Session.SetJson("cart", Cart);
			}
			return View("Cart", Cart);
		}


		public IActionResult RemoveCart(int id)
		{
			Book? book = _context.Book?.FirstOrDefault(b => b.Id == id);
			if (book != null)
			{
				Cart = HttpContext.Session.GetJson<Cart>("cart");
				Cart?.RemoveLine(book);
				HttpContext.Session.SetJson("cart", Cart);
			}
			return View("Cart", Cart);
		}

		public IActionResult Checkout()
		{

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			Cart = HttpContext.Session.GetJson<Cart>("cart");

			if (Cart != null && Cart.Lines.Any())
			{
				var order = new Order
				{
					UserId = userId,
					Price = Cart.ComputeTotalValue(),
					Date = DateTime.Now
				};

				_context.Order.Add(order);
				_context.SaveChanges();

				foreach (var line in Cart.Lines)
				{
					var orderDetail = new OrderDetail
					{
						BookId = line.Book.Id,
						OrderId = order.Id,
						Quantity = line.Quantity
					};

					_context.OrderDetail.Add(orderDetail);
				}

				_context.SaveChanges();

				// Remove the cart from the session after successful checkout
				HttpContext.Session.Remove("cart");

				return RedirectToAction("Index", "Home");
			}
			else
			{

				return View("Cart", Cart);
			}
		}

	}

}
