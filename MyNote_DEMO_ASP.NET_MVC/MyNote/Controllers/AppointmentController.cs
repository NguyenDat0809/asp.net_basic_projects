using Microsoft.AspNetCore.Mvc;

namespace MyNote.Controllers
{
    public class AppointmentController : Controller
    {
        public IActionResult Index()
        {
            
            return View();
            //return Ok(todaysDate);
        }
    }
}
