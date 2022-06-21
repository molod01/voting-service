using System;
using System.Collections.Generic;

namespace voterilka.Model
{
    public class Vote
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Variant> Variants { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsAnonymous { get; set; }
        public Vote()
        {
            Variants = new List<Variant>();
            Comments = new List<Comment>();
            CreationDate = DateTime.Now;
            IsAnonymous = false;
        }
    }
}
