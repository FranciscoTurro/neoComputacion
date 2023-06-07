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

        public IActionResult Index()
        {
            List<Post> postsList = _context.Posts.ToList();
            List<Post> postsListToSend = postsList.Select(post => new Post
            {
                Id = post.Id,
                Title = post.Title,
                Image = post.Image,
                Content = truncateString(post.Content)
            }).ToList();

            return View(postsListToSend);
        }


        public string truncateString(string input)
        {
            if (input.Length <= 50)
            {
                return input;
            }
            else
            {
                return input.Substring(0, 50) + "...";
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}