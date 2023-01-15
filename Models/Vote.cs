namespace Project.Models
{
    public class Vote
    {
        public int VoteId { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
        public int VoteValue { get; set; }
    }
}
