using neoComputacion.Models;

namespace neoComputacion.ViewModel
{
    public class PostVM
    {
        public Post oPost { get; set; }
        public IFormFile? PhotoPath { get; set; }
    }
}
