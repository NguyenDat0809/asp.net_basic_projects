using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UploadFile.Models;
using UploadFile.Models.ModelsVM;

namespace UploadFile.Controllers
{
    public class FileController : Controller
    {
        private readonly ILogger<FileController> _logger;
        private readonly IWebHostEnvironment _webHost;
        //fake data with static "database"
        public static List<Profile> _dbContext = new List<Profile>() {
                new Profile() {Id = 1, Image = "1.png", Name="ng1"},
                new Profile() {Id = 2, Image = "2.png", Name="ng2"},
            };
        public FileController(ILogger<FileController> logger, IWebHostEnvironment webHost)
        {
            _logger = logger;
            _webHost = webHost;
        }
        [HttpGet]
        public IActionResult Single()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Single(IFormFile file)
        {
            string uploadFolder = Path.Combine(_webHost.WebRootPath, "uploads");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            string fullPath = Path.GetFullPath(file.FileName);
            string fileName = Path.GetFileName(file.FileName);
            string fileSavePath = Path.Combine(uploadFolder, fileName);

            FileStream stream = new FileStream(fileSavePath, FileMode.Create);

            await file.CopyToAsync(stream);


            ViewBag.fullPath = fullPath; //đường dẫn của file được upload từ folder UploadedFile
            ViewBag.fileName = fileName; //tên  của file được upload
            ViewBag.fileSavePath = fileSavePath; //đường dẫn của file một cách cụ thể - bao gồm cả wwwroot
            return View();

        }

        public IActionResult Multiple()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Multiple(List<IFormFile> files)
        {
            //chỉ định nơi lưu trữ - trong trường hợp này là folder uploads trong wwwroot
            //cũng như là tạo đường dẫn dến đó
            string uploadsFolder = Path.Combine(_webHost.WebRootPath, "uploads"); //đường dẫn được tạo như sau: wwwroot/uploads

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);  //tạo thư mục nếu chưa có folder đó
            }
            foreach (var file in files)
            {
                //string fullPath = Path.GetFullPath(file.FileName); 
                string fileName = Path.GetFileName(file.FileName); //trích xuất tên tệp mà ko có đường dẫn
                string fileSavePath = Path.Combine(uploadsFolder, fileName); //tạo đường dẫn đầy đủ đến folder uploads
                                                                             //đường dẫn như sau: wwwroot/uploads/<filename>

                using (FileStream stream = new FileStream(fileSavePath, FileMode.Create) /*tạo đường dẫn luồng tệp đến folder uploads*/)
                {
                    await file.CopyToAsync(stream);
                }


                ViewBag.filesName += string.Format("<b> {0} </b> uploaded succesfully. <br/>", fileName);
            }
            return View();

        }

        public IActionResult SaveMultipleFile()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveMultipleFile(ProfileVM profileVM)
        {
            
            //handle saveing images to folder uploads
            string uploadsFolder = Path.Combine(_webHost.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            foreach (var file in profileVM.Image)
            {
                Profile add = new Profile();
                string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string fileSavePath = Path.Combine(uploadsFolder, fileName);

                //save the path to object Profile
                add.Image = fileName;

                using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                //save to database
                var index = _dbContext.Count() + 1;
                add.Id = index;
                add.Name = "ng" + index;
                _dbContext.Add(add);
            }

            return RedirectToAction("Profiles", "File");
        }

        public IActionResult Profiles()
        {
         
            return View(_dbContext);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
