using voterilka.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
using voterilka.Crypto;

namespace voterilka.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly VoterDbContext _context;

        public AuthenticationController(VoterDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Profile()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var logined_user = await _context.Users.FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
                return View(logined_user);
            }
            else return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                string error = "Пользователь не найден";
                return Redirect("/Home/Error?error=" + error);
            }
            else if (ModelState.IsValid)
            {

                if (await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username) == null)
                {
                    user.Password = Hasher.GetHash(user.Password);
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    await Authenticate(user.Username);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Home", "Index");
        }

        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult PasswordReset()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordReset(User user)
        {
            string error;
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                var usr = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
                usr.Password = Hasher.GetHash("default");
                await _context.SaveChangesAsync();
            }
            else
            {
                error = "Пользователь не найден";
                return Redirect("/Home/Error?error=" + error);
            }
            return Redirect("/Authentication/SignIn");
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(User user, int? id)
        {
            string error;
            user.Password = Hasher.GetHash(user.Password);
            if (!await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                error = "Пользователь не найден";
                return Redirect("/Home/Error?error=" + error);
            }
            else if (await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == user.Password) != null)
            {
                await Authenticate(user.Username); // аутентификация
                if (user.Username == "admin") return RedirectToAction("Panel", "AdminPanel");
                else if(id != null) return Redirect("/Vote/Poll?id=" + id);
                else return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            error = "Неверный пароль";
            return Redirect("/Home/Error?error=" + error);
        }
        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLogin(string userName)
        {
            await Logout();
            await Authenticate(userName);
            return RedirectToAction("View", "Profile");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Authentication/SignIn");
        }
    }
}