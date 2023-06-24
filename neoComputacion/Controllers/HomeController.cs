using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index(int page = 1, int postsPerPage = 8)
        {
            int postsCount = _context.Posts.Count();

            int pagesCount = (int)Math.Ceiling((double)postsCount / postsPerPage);

            List<Post> postsList = _context.Posts
                .OrderByDescending(post => post.CreationDate)
                .Skip((page - 1) * postsPerPage)
                .Take(postsPerPage)
                .ToList();

            // creo un objeto de paginacion, que tiene la lista de posteos para mostrar 
            //y informacion necesaria para poder navegar entre paginas
            var paginationViewModel = new PaginationVM<Post>
            {
                Posts = postsList,
                PageNumber = page,
                PageSize = postsPerPage,
                TotalPages = pagesCount
            };

            return View(paginationViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
