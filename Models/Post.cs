namespace Project.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public User User { get; set; }
        public bool Edited { get; set; } = false;
        public List<Vote> Votes { get; set; }

    }
}
