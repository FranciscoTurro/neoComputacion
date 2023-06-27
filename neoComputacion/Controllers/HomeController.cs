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
                //Post.Categories viene de una tabla intermedia asi que no esta "cargado",
                //necesito incluirlo en mi busqueda
                .OrderByDescending(post => post.CreationDate);

            if (categoryId.HasValue)
            {
                //si hay una categoria por la que el usuario quiere filtrar agrego a 
                //query que quiero solamente posteos que satisfagan la condicion en Any()
                //es decir, que son la misma categoria (comparten id)
                query = query.Where(post => post.Categories.Any(category => category.Id == categoryId));
            }

            //cuento los posteos y las paginas para la paginacion
            int postsCount = query.Count();
            int pagesCount = (int)Math.Ceiling((double)postsCount / postsPerPage);

            //tomo la cantidad necesaria de posteos de query y la paso a lista
            List<Post> postsList = query
                .Skip((page - 1) * postsPerPage)
                .Take(postsPerPage)
                .ToList();

            //creo un objeto paginacion con info necesaria para paginar
            var paginationViewModel = new PaginationVM<Post>
            {
                Posts = postsList,
                PageNumber = page,
                PageSize = postsPerPage,
                TotalPages = pagesCount
            };

            //le paso todas las categorias existentes a la pagina
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
