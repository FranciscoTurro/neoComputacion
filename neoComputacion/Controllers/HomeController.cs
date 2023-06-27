using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using neoComputacion.Models;
using System.Diagnostics;

namespace neoComputacion.Controllers
{
    public class HomeController : Controller
    {
        private readonly NeoCompDbContext _context;
        public HomeController(NeoCompDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, int postsPerPage = 8, int? categoryId = null)
        {
            IQueryable<Post> query = _context.Posts.Include(p => p.Categories)
                .OrderByDescending(post => post.CreationDate);

            if (categoryId.HasValue)
            {
                query = query.Where(post => post.Categories.Any(category => category.Id == categoryId));
            }

            int postsCount = query.Count();
            int pagesCount = (int)Math.Ceiling((double)postsCount / postsPerPage);

            List<Post> postsList = query
                .Skip((page - 1) * postsPerPage)
                .Take(postsPerPage)
                .ToList();

            var paginationViewModel = new PaginationVM<Post>
            {
                Posts = postsList,
                PageNumber = page,
                PageSize = postsPerPage,
                TotalPages = pagesCount
            };

            ViewBag.Categories = _context.Categories.ToList();

            return View(paginationViewModel);
        }


        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
