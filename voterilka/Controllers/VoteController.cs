using Microsoft.AspNetCore.Mvc;
using voterilka.Model;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace voterilka.Controllers
{
    public class VoteController : Controller
    {
        private readonly VoterDbContext _context;

        public VoteController(VoterDbContext context)
        {
            _context = context;
        }
        private bool isVoted()
        {
            foreach (var variant in  _context.Variants.ToList())
            {
                foreach (var voter in variant.Voters.ToList())
                {
                    if (voter.Username == User.Identity.Name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public IActionResult Showcase()
        {  
            foreach (Vote vote in _context.Votes.ToList())
            {
                _context.Entry(vote).Collection(x => x.Variants).Load();
                foreach (Variant variant in vote.Variants)
                {
                    _context.Entry(variant).Collection(x => x.Voters).Load();
                }
            }
            return View(_context.Votes);
        }

        public async Task<IActionResult> Poll(int id)
        {
            var vote = _context.Votes.Find(id);
     
            if (vote != null)
            {
                await _context.Entry(vote).Collection("Variants").LoadAsync();
                foreach (var variant in vote.Variants)
                {
                    await _context.Entry(variant).Collection("Voters").LoadAsync();
                }

                if(isVoted())//если проголосовано
                {
                    return View("CompletePoll", vote);
                }
                else
                {
                    return View("Poll", vote);
                }
                
            }
            else return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Polling(string voteid, string variant)
        {
            if (User.Identity.IsAuthenticated)
            {
                Vote vote = await _context.Votes.FindAsync(int.Parse(voteid));
                _context.Entry(vote).Collection("Variants").Load();
                var chosen_option = await _context.Variants.FindAsync(int.Parse(variant));

                var user = await _context.Users.FirstAsync(u => u.Username == HttpContext.User.Identity.Name);

                if (user != null)
                {
                    if (!chosen_option.Voters.Contains(user))//если уже содержит такого юзера в списке проголосовавших
                    {
                        foreach (var item in vote.Variants)
                        {
                            if (item.Voters.Contains(user))
                            {
                                item.Voters.Remove(user);
                            }
                        }
                        chosen_option.Voters.Add(user);
                        await _context.SaveChangesAsync();
                    }
                }
                return Redirect("/Vote/Poll?id=" + vote.Id);
            }
            else
            {
                return Redirect("/Authentication/SignIn?id=" + voteid);
            }
        }
    }
}
