using Microsoft.AspNetCore.Mvc;

namespace AutoGenerateAPI.Controllers
{
    public class GETController : Controller
    {
        [Route("getApiTest")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
