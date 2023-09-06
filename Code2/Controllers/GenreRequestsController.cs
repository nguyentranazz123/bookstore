using Code2.Data;
using Code2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Code2.Controllers
{
    [Authorize(Roles = "Admin, StoreOwner")]
    public class GenreRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public GenreRequestsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: GenreRequests
        public async Task<IActionResult> Index()
        {
            var genreRequests = await _context.GenreRequest.ToListAsync();
            ViewBag.UserManager = _userManager;
            return View(genreRequests);
        }

        // GET: GenreRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GenreRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GenreName,IsApproved")] GenreRequest genreRequest)
        {
            if (ModelState.IsValid)
            {
                // Check if the genre name already exists in the Genre table
                var existingGenre = await _context.Genre.FirstOrDefaultAsync(g => g.Name == genreRequest.GenreName);

                if (existingGenre != null)
                {
                    // Genre name already exists, show a notification to the store owner
                    ModelState.AddModelError("GenreName", "Genre name already exists.");
                    return View(genreRequest);
                }

                // Get the currently logged-in user's ID
                string requestedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Set the RequestedByUserId property
                genreRequest.RequestedByUserId = requestedByUserId;

                _context.Add(genreRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genreRequest);
        }

        [Authorize(Roles = "Admin")]
        // POST: GenreRequests/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var genreRequest = await _context.GenreRequest.FindAsync(id);

            if (genreRequest != null)
            {
                genreRequest.IsApproved = true;
                var newGenre = new Genre
                {
                    Name = genreRequest.GenreName
                };
                _context.Genre.Add(newGenre);
                await _context.SaveChangesAsync();
            }

            // Pass the genreRequest data to the view
            return View("ApproveGenreRequests", genreRequest);
        }

        [Authorize(Roles = "Admin")]
        // POST: GenreRequests/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var genreRequest = await _context.GenreRequest.FindAsync(id);

            if (genreRequest != null)
            {
                _context.GenreRequest.Remove(genreRequest);
                await _context.SaveChangesAsync();
            }

            return View("RejectGenreRequests");
        }

        private bool GenreRequestExists(int id)
        {
            return _context.GenreRequest.Any(e => e.Id == id);
        }
    }
}
