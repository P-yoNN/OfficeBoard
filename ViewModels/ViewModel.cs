using Project.Models;

namespace Project.ViewModels
{
    public class ViewModel
    {
        public User User { get; set; }
        public Post Post { get; set; }
        public Dictionary<Post, int> PostWithValue{ get; set; }
    }
}
