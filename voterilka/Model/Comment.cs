using System;

namespace voterilka.Model
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Vote Vote { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public bool isDeleted { get; set; }
        public bool isReply { get; set; }
        public int? RepliedCommentId { get; set; }
    }
}
