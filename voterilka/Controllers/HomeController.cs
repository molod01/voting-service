using Microsoft.AspNetCore.Mvc;
using voterilka.Model;

namespace voterilka.Controllers
{
    public class HomeController : Controller
    {
        private readonly VoterDbContext _context;

        public HomeController(VoterDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error(string error)
        {

            return View("Error", error);
        }
    }
}
