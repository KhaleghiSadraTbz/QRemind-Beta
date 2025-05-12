using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    public class HTMLServeController : Controller
    {
        [Route("")]
        public IActionResult index()
        {
            Console.WriteLine("Served index.html");
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/html");
        }
    }
}
