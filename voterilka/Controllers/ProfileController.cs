using Microsoft.AspNetCore.Mvc;
using voterilka.Model;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using voterilka.Crypto;

namespace voterilka.Controllers
{
    public class ProfileController : Controller
    {
        private readonly VoterDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        public ProfileController(VoterDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        public IActionResult Index()
        {
            var profile = _context.Users.First(u => u.Username == User.Identity.Name);
            return View(profile);
        }
        public IActionResult Edit()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile avatar)
        {
            if (avatar != null)
            {
                string path = "/images/" + avatar.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await avatar.CopyToAsync(fileStream);
                }
                 _context.Users.FirstOrDefault(u => u.Username == User.Identity.Name).PicUrl = path;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(User user, string OldPassword)
        {
            if(user != null)
            {
                var profile = await _context.Users.FirstAsync(u => u.Username == User.Identity.Name);
                if(profile != null)
                {
                    if(profile.Password == Hasher.GetHash(OldPassword))
                    {
                        profile.Password = Hasher.GetHash(user.Password);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> ChangeNickname(string newnick)
        {
            if (newnick != null && !_context.Users.Any(u => u.Username == newnick))
            {
                //_context.Users.FirstOrDefaultAsync(u => u.Username == User.Identity.Name).Result.Username = newnick;
                //await _context.SaveChangesAsync();
                //await _userManager.
            }
            return View();
        }
    }
}
