using Code2.Data;
using Code2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Code2.Controllers
{
	public class HomeController : Controller
	{

		private readonly ApplicationDbContext _context;

		public HomeController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var genres = _context.Genre.ToList();
			ViewBag.Genres = genres;

			var books = _context.Book.Include(b => b.Genre).ToList();
			return View(books);
		}

		public IActionResult Help()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public async Task<IActionResult> Search(string query)
		{
			ViewBag.Genres = await _context.Genre.ToListAsync();
			if (string.IsNullOrEmpty(query))
			{
				return View("Index", _context.Book.ToList());
			}
			var books = await _context.Book
				.Where(b => b.Name.Contains(query))
				.ToListAsync();

			return View("Index", books);
		}
		public async Task<IActionResult> Filter(int genre)
		{
			ViewBag.Genres = await _context.Genre.ToListAsync(); // Populate the genres dropdown

			if (genre == 0)
			{
				ViewBag.SelectedGenreId = 0; // No genre selected
				return View("Index", await _context.Book.ToListAsync());
			}

			var books = await _context.Book
				.Where(b => b.GenreId == genre)
				.ToListAsync();

			ViewBag.SelectedGenreId = genre; // Set the selected genre ID for rendering the view
			return View("Index", books);
		}


	}
}