using Microsoft.AspNetCore.Mvc;
using voterilka.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace voterilka.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly VoterDbContext _context;
        private string error;
        public AdminPanelController(VoterDbContext context)
        {
            _context = context;
            error = "У вас нет админских прав";
        }
        private bool checkAdminPermissions()
        {
            if (User.Identity.IsAuthenticated && User.Identity.Name == "admin")
            {
                return true;
            }
            else return false;
        }
        public IActionResult Panel()
        {
            if (checkAdminPermissions())
            {
                return View();
            }
            else return Redirect("/Home/Error?error=" + error);
        }

        public IActionResult Add()
        {
            if (checkAdminPermissions())
            {
                return View();
            }
            else return Redirect("/Home/Error?error=" + error);
        }
        public IActionResult Remove()
        {
            return View(_context.Votes.ToList());
        }
        public IActionResult EditPicker()
        {
            return View(_context.Votes.ToList());
        }
        public IActionResult EditPanel()
        {
            if (checkAdminPermissions())
            {
                return View();
            }
            else return Redirect("/Home/Error?error=" + error);
        }
        public async Task<IActionResult> Userslist()
        {
            if (User.Identity.IsAuthenticated && User.Identity.Name == "admin")
            {
                foreach (User user in _context.Users.ToList())
                {
                    await _context.Entry(user).Collection(x => x.Variants).LoadAsync();
                }
                return View(_context.Users);
            }
            else return Redirect("/Home/Error");
        }

        [HttpPost]
        public async Task<IActionResult> Add(Vote vote, string[] options)
        {
            _context.Votes.Add(vote);
            var variants = options.SkipLast(1).ToList();
            foreach (var variant in variants)
            {
                Variant var = new Variant { Name = variant, Vote = vote };
                _context.Variants.Add(var);
            }

            await _context.SaveChangesAsync();
            return Redirect("/Vote/Showcase");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string title)
        {
            Vote vote = await _context.Votes.FirstAsync(v => v.Title == title);
            if (vote != null)
            {
                var variants = _context.Variants.Where(v => v.Vote == vote).ToList();
                _context.Variants.RemoveRange(variants);
                //foreach (var variant in variants)
                //{
                //    _context.Variants.Remove(variant);
                //}
                _context.Votes.Remove(vote);
                await _context.SaveChangesAsync();
                return Redirect("/Vote/Showcase");
            }
            else
                return Redirect("/Home/Error?error=Ошибка удаления");
        }

        [HttpPost]
        public IActionResult EditPicker(string title)
        {
            Vote vote = _context.Votes.First(v => v.Title == title);
            _context.Entry(vote).Collection("Variants").Load();
            return View("EditPanel", vote);
        }

        [HttpPost]
        public async Task<IActionResult> EditPanel(Vote vote, string[] options)
        {
            var variants = options.SkipLast(1).ToList();
            var old = _context.Variants.Where(i => i.Vote == vote).ToList();

            for (int i = 0; i < old.Count; i++)
            {
                if (!variants.Contains(old[i].Name))
                {
                    _context.Variants.Remove(old[i]);
                }
            }



            _context.Variants.RemoveRange(old);

            foreach (var variant in variants)
            {
                Variant var = new Variant { Name = variant, Vote = vote };
                _context.Variants.Add(var);
            }

            _context.Votes.Update(vote);
            await _context.SaveChangesAsync();

            return Redirect("/Vote/Poll?id=" + vote.Id);
        }
    }
}
