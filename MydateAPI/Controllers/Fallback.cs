
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace MydateAPI.Controllers
{
    //Use Controller instead of ControllerBase to support view
    public class Fallback : Controller
    {
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");
        }
    }
}
