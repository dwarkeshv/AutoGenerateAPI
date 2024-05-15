using Microsoft.AspNetCore.Mvc;

namespace AutoGenerateAPI.Controllers
{
    public class POSTController : Controller
    {
        [Route("postApiTest")]
        [HttpPost]
        public IActionResult Index()
        {
            return View();
        }
    }
}
