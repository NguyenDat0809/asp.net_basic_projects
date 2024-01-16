using Microsoft.AspNetCore.Mvc;
using MyNote.Data;
using MyNote.Models;

namespace MyNote.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ItemController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Item> objList = _context.Items;
            return View(objList);
        }
        //Get - Create
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
        [ValidateAntiForgeryToken] //-> tăng security, những đứa có thẩm quyền hoặc login mới dc
        public IActionResult Create(Item obj)
		{
            _context.Items.Add(obj);
            _context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
