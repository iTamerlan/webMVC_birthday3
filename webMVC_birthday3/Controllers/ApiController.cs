using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebBirthdayMVC.Controllers
{
    public class ApiController : Controller
    {
        // GET: UserController
        [HttpGet]
        public ActionResult users()
        {
            return View();
        }

        /*[HttpGet]
        public ActionResult users()
        {
            return View();
        }*/

    }
}
