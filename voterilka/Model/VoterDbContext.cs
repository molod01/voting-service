using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace voterilka.Model
{
    public sealed class VoterDbContext : DbContext
    {
        public VoterDbContext(DbContextOptions<VoterDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Vote> Votes { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            User adminUser = new User { Id = 1, Username = "admin", Password = "9v3/5IyQjesPTDvTbAMucg==" };
            User molod01 = new User { Id = 2, Username = "molod01", Password = "mZAtHyc11waOFEZt94jwNw==", PicUrl= "https://i1.sndcdn.com/avatars-LRKQlOaSxCxr99ew-bjnstw-t200x200.jpg" };

            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            modelBuilder.Entity<User>().HasData(new User[] { molod01 });

        }
    }
}
