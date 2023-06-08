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
            Post post = _context.Posts.Find(id);

            return post;
        }

        [HttpGet]
        public IActionResult DeletePost(int id)
        {
            Post post = getOnePost(id);

            _context.Posts.Remove(post);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public IActionResult DetailPost(int id)
        {
            Post post = getOnePost(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpGet]
        public IActionResult EditPost(int id)
        {
            Post post = getOnePost(id);

            if (post == null)
            {
                return NotFound();
            }

            PostVM postVM = new PostVM
            {
                oPost = post
            };

            return View(postVM);
        }


        [HttpPost]
        public IActionResult EditPost(PostVM postModel)
        {
            string fileName = UploadFile(postModel);

            Post existingPost = _context.Posts.Find(postModel.oPost.Id);

            if (existingPost == null)
            {
                return NotFound();
            }//ya se que el posteo existe pero necesito traer el post original para editarlo

            if (postModel.oPost.Title != null)
            {
                existingPost.Title = postModel.oPost.Title;
            }

            if (fileName != null)
            {
                existingPost.Image = fileName;
            }

            if (postModel.oPost.Content != null)
            {
                existingPost.Content = postModel.oPost.Content;
            }

            _context.Posts.Update(existingPost);
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
