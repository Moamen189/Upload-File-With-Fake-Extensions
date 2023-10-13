using FileWithFakeExtensions.Data;
using Microsoft.AspNetCore.Mvc;

namespace FileWithFakeExtensions.Controllers
{
    public class fileController : Controller
    {
        private readonly ApplicationDBContext _context;

        public fileController(ApplicationDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var Files = _context.UploadedFiles.ToList();
            return View(Files);
        }

        public IActionResult Upload()
        {
            return View();
        }
    }
}
