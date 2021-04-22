using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Services;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly LogService _logService;

        public HomeController(LogService logService)
        {
            _logService = logService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/home/list.dll")]
        public IActionResult List([FromBody] SearchLogModel model)
        {
            
            var data = _logService.Get(model);
            return Json(data);
        }

        [HttpPost("/home/appnames.dll")]
        public IActionResult AppNames()
        {
            var data = _logService.GetAppNames();
            return Json(data);
        }
    }
}
