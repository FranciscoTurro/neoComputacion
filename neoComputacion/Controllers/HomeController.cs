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
                .OrderByDescending(post => post.Id)  // uso el id para determinar cual post es el mas nuevo (no estoy del todo seguro si es la solucion ideal)
                .Skip((page - 1) * postsPerPage)
                .Take(postsPerPage)
                .ToList();

            List<Post> postsListToSend = postsList.Select(post => new Post
            {
                Id = post.Id,
                Title = post.Title,
                Image = post.Image,
                Content = truncateString(post.Content)
            }).ToList();

            // creo un objeto de paginacion, que tiene la lista de posteos para mostrar 
            //y informacion necesaria para poder navegar entre paginas
            var paginationViewModel = new PaginationVM<Post>
            {
                Posts = postsListToSend,
                PageNumber = page,
                PageSize = postsPerPage,
                TotalItems = postsCount,
                TotalPages = pagesCount
            };

            return View(paginationViewModel);
        }


        public string truncateString(string input)
        {
            int numberOfLetters = 70;

            if (input.Length <= numberOfLetters)
            {
                return input;
            }
            else
            {
                return input.Substring(0, numberOfLetters) + "...";
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
