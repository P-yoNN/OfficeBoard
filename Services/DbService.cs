using Project.Models;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Services
{
    public class DbService
    {
        public ApplicationContext _context { get; set; }
        public DbService(ApplicationContext db)
        {
            _context = db;
        }


        public User? FindUser(string User)
        {
            return _context.Users.FirstOrDefault(u => u.UserName.Equals(User));
        }
        public User GetUser(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId.Equals(userId));
        }
        public void RegisterUser(User user)
        {
            if (!_context.Users.Contains(FindUser(user.UserName)))
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
        }
        public void EditUser(User editeUser)
        {
            User user = GetUser(editeUser.UserId);
            user.UserName = editeUser.UserName;
            user.Name = editeUser.Name;
            user.Surname = editeUser.Surname;
            user.Email = editeUser.Email;
            
            _context.SaveChanges();
        }


        public Post GetPost(int Id)
        {
            return _context.Posts.FirstOrDefault(p => p.Id.Equals(Id));
        }
        public List<Post> GetAllPosts()
        {
            return _context.Posts.Include(p => p.Votes).Include(p => p.User).ToList();
        }
        public void CreatePost(Post post, int userId)
        {
            
            post.User = _context.Users.FirstOrDefault(u => u.UserId.Equals(userId));

            _context.Posts.Add(post);
            _context.SaveChanges();
        }
        public void EditPost(Post editedpost)
        {
            Post post = GetPost(editedpost.Id);
            post.Header = editedpost.Header;
            post.Body = editedpost.Body;
            post.Edited = true;
            _context.SaveChanges();
        }


        public Dictionary<Post, int> GetPostsWithVoteValue(List<Post> posts)
        {
            Dictionary<Post, int> ValuedPosts = new Dictionary<Post, int>();
            foreach (Post post in posts)
            {
                ValuedPosts.Add(post, CountVotes(post));
            }
            return ValuedPosts;
        }


        public void Vote(int value, int userId, int postId)
        {
            Vote? vote = GetVote(userId, postId);
            if (vote != null)
            {
                if (value == vote.VoteValue)
                {
                    _context.Votes.Remove(vote);

                }
                else
                {
                    vote.VoteValue = value;
                }
                _context.SaveChanges();
            }
            else
            
            {
                _context.Votes.Add(new Vote()
                {
                    User = GetUser(userId),
                    Post = GetPost(postId),
                    VoteValue = value
                }) ;
                _context.SaveChanges();
            }
        }
        public int CountVotes(Post post)
        {
            int value = 0;
            if (post.Votes != null)
            {
                foreach (Vote v in post.Votes)
                {
                    if (v.VoteValue.Equals(1))
                    {
                        value++;
                    }
                    else
                        value--;
                }
            }
            return value;
        }
        public Vote? GetVote(int userId, int postId)
        {
            return _context.Votes.FirstOrDefault(v => v.User.UserId.Equals(userId) && v.Post.Id == postId);
        }
    }
}