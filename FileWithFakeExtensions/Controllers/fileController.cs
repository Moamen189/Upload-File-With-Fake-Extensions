using FileWithFakeExtensions.Data;
using FileWithFakeExtensions.Models;
using FileWithFakeExtensions.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FileWithFakeExtensions.Controllers
{
    public class fileController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public fileController(ApplicationDBContext context , IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UploadFiles(UploadFilesFormViewModel model)
        {
            // validate file extentions and size 

            List<UploadedFile> uploadedFiles = new ();

            foreach (var file in model.Files)
            {
            var fakeFileName = Path.GetRandomFileName();
                 UploadedFile uploadedFile = new(){
                     FileName= file.FileName,
                     ContentType= file.ContentType,
                     StoredFileName= fakeFileName
                 };
                var path = Path.Combine(webHostEnvironment.WebRootPath, "Uploads", fakeFileName);
                using FileStream fileStream = new(path , FileMode.Create);

                file.CopyTo(fileStream);

                uploadedFiles.Add(uploadedFile);
            }
            _context.AddRange(uploadedFiles);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]

        public IActionResult DownloadFile(string FileName)
        {
            var uploadedFile = _context.UploadedFiles.SingleOrDefault(x => x.StoredFileName == FileName);

            if(uploadedFile is null)
                return NotFound();

            var path = Path.Combine(webHostEnvironment.WebRootPath, "Uploads", FileName);

            MemoryStream memoryStream= new();

            using FileStream fileStream = new(path , FileMode.Open);

            fileStream.CopyTo(memoryStream);

            memoryStream.Position = 0;

            return File(memoryStream, uploadedFile.ContentType, uploadedFile.FileName);
        }
    }
}
