using Microsoft.AspNetCore.Mvc;

namespace OnlineConsulting.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/status/{status:int}")]
        public IActionResult NotFoundPage(int status)
        {
            return status switch
            {
                404 => View("NotFoundPage"),
                403 => View("ForbiddenPage"),
                _ => View("ServerError"),
            };
        }

    }
}
