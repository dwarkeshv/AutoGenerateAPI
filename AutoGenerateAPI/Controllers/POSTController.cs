using AutoGenerateAPI.Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoGenerateAPI.Controllers
{
    [Route("api/[controller]")]
    public class POSTController : Controller
    {
       

        [Route("createTableForHeaders")]
        [HttpPost]
        public IActionResult addHeaders()
        {

            return null;
        }

        [Route("createTableForPersonalInformation")]
        [HttpPost]
        public IActionResult addPersonalInfo()
        {
            return null;
        }

        [Route("createTableForAdditionalInformation")]
        [HttpPost]
        public IActionResult addAddittionalInfo()
        {
            return null;
        }


    }
}
//[Route("insertTableForHeaders")]
//[HttpPost]
//public IActionResult addApplication(HeroTable data)
//{

//    return null;
//}