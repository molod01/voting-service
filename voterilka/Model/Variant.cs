using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace voterilka.Model
{
    public class Variant
    {
        public int Id { get; set; }
        public int? VoteId { get; set; }
        [ForeignKey("VoteId")]
        public virtual Vote Vote { get; set; }
        public string Name { get; set; }
        public ICollection<User> Voters { get; set; }
        public Variant()
        {
            Voters = new List<User>();
        }
    }
}
