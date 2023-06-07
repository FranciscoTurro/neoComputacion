using Microsoft.AspNetCore.Mvc;
using neoComputacion.Models;
using neoComputacion.ViewModel;
using System.Diagnostics;

namespace neoComputacion.Controllers
{
    public class PostController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly NeoCompDbContext _context;

        public PostController(NeoCompDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
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

        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePost(PostVM postModel)
        {
            string fileName = UploadFile(postModel);

            Post post = new Post()
            {
                Id = postModel.oPost.Id,
                Title = postModel.oPost.Title,
                Image = fileName,
                Content = postModel.oPost.Content,
            };

            _context.Posts.Add(post);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");

        }

        private string UploadFile(PostVM postModel)
        {
            string fileName = null;
            if (postModel.PhotoPath != null)
            {
                string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "images");
                fileName = Guid.NewGuid().ToString() + "-" + postModel.PhotoPath.FileName;
                string fileRoute = Path.Combine(uploadDir, fileName);

                using (var fileStream = new FileStream(fileRoute, FileMode.Create))
                {
                    postModel.PhotoPath.CopyTo(fileStream);
                }
            }
            return fileName;
        }

        private Post getOnePost(int id)
        {
            Post post = _context.Posts.Find(id);

            return post;
        }

        public IActionResult DeletePost(int id)
        {
            Post post = getOnePost(id);

            _context.Posts.Remove(post);
            _context.SaveChanges();

            return RedirectToAction("Index", "Post");

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
