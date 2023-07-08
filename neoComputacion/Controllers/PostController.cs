using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public IActionResult CreatePost()
        {
            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public IActionResult CreatePost(PostVM postModel, string[] categories)
        {
            if (categories == null || categories.Length == 0)
                categories = new[] { "4" }; // categoria por default si el usuario no elige, PC

            string fileName = UploadFile(postModel);

            Post post = new Post()
            {
                Id = postModel.oPost.Id,
                Title = postModel.oPost.Title,
                Image = fileName,
                Content = postModel.oPost.Content,
                Categories = new List<Category>()
            };

            foreach (var categoryId in categories)
            {
                //por cada una de las categorias elegidas por el usuario se agregan
                //a las categorias de este posteo. entity framework las agrega a
                //la tabla intermedia PostsCategory (creo)
                var category = _context.Categories.Find(Int32.Parse(categoryId));
                if (category != null)
                {
                    post.Categories.Add(category);
                }
            }

            _context.Posts.Add(post);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");

        }

        private string UploadFile(PostVM postModel)
        {
            string fileName = null;
            if (postModel.PhotoPath != null)
            {
                string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "images/postImages");
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
            Post post = _context.Posts
                .Include(p => p.Categories)
                .FirstOrDefault(p => p.Id == id);

            return post;
        }


        [HttpGet]
        public IActionResult DeletePost(int id)
        {
            Post post = getOnePost(id);

            if (post == null)
                return NotFound();

            // tengo que borrar manualmente las entradas en la tabla intermedia, hay una opcion en el
            //entity framework para borrar en cascada, pero no estaba funcionando correctamente

            var postCategories = _context.Entry(post).Collection(p => p.Categories).Query().ToList();
            foreach (var category in postCategories)
            {
                post.Categories.Remove(category);
            }

            _context.Posts.Remove(post);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult DetailPost(int id)
        {
            Post post = getOnePost(id);

            if (post == null)
                return NotFound();

            return View(post);
        }

        [HttpGet]
        public IActionResult EditPost(int id)
        {
            Post post = getOnePost(id);

            if (post == null)
                return NotFound();

            PostVM postVM = new PostVM
            {
                oPost = post
            };

            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;

            return View(postVM);
        }


        [HttpPost]
        public IActionResult EditPost(PostVM postModel, string[] categories)
        {
            string fileName = UploadFile(postModel);

            Post existingPost = _context.Posts.Include(p => p.Categories).FirstOrDefault(p => p.Id == postModel.oPost.Id);

            if (existingPost == null)
                return NotFound();

            existingPost.Title = postModel.oPost.Title;
            existingPost.Content = postModel.oPost.Content;

            if (categories != null && categories.Length > 0)
            {
                existingPost.Categories.Clear();

                foreach (var categoryId in categories)
                {
                    var category = _context.Categories.Find(Int32.Parse(categoryId));
                    if (category != null)
                    {
                        existingPost.Categories.Add(category);
                    }
                }
            }

            if (fileName != null)
                existingPost.Image = fileName;

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
