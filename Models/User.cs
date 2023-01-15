namespace Project.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string  Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public List<Post> Posts { get; set; }
        public List<Vote> Votes { get; set; }
    }
}
