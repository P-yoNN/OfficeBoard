using Project.Services;
using Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        public ViewModel Hmv { get; set; }
        public DbService Ds { get; set; }
        public HomeController(DbService dbService)
        {
            Ds = dbService;
            Hmv = new ViewModel();
            List<Post> posts = Ds.GetAllPosts();
            Hmv.PostWithValue = Ds.GetPostsWithVoteValue(posts);
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index", Hmv);
        }


        //User part of controller
        [HttpGet("User")]
        public IActionResult WievUser()
        {
            if (null != Request.Cookies["ActiveUser"])
            {
                int userId = int.Parse(Request.Cookies["ActiveUser"]);
                Hmv.User = Ds.GetUser(userId);
                List<Post> userPosts = Ds.GetAllPosts().Where(u => u.User.UserId == userId).ToList();
                Hmv.PostWithValue = Ds.GetPostsWithVoteValue(userPosts);
                return View("User", Hmv);
            }
            else return View("Login");
        }

        [HttpGet("Register")]
        public IActionResult RegisterUser()
        {
            return View("Register");
        }

        [HttpPost("Register")]
        public IActionResult RegisterUser(User user)
        {
            Ds.RegisterUser(user);
            return View("Login");
        }

        [HttpGet("User/{userId}")]
        public IActionResult User(int userId)
        {
            Hmv.User = Ds.GetUser(userId);
            List<Post> userPosts = Ds.GetAllPosts().Where(u => u.User.UserId == userId).ToList();
            Hmv.PostWithValue = Ds.GetPostsWithVoteValue(userPosts);
            return View("User", Hmv);
        }

        [HttpGet("EditUser/{userId}")]
        public IActionResult EditUser(int userId)
        {
            Hmv.User = Ds.GetUser(userId);
            if (null != Request.Cookies["ActiveUser"] && userId == int.Parse(Request.Cookies["ActiveUser"]))
            {
                return View("EditUser", Hmv);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost("EditUser/{userId}")]
        public IActionResult WievUser(int userID)
        {
            User user = Ds.GetUser(userID);

            Ds.EditUser(user);
            Hmv.User = Ds.GetUser(user.UserId);
            List<Post> userPosts = Ds.GetAllPosts().Where(u => u.User.UserId == user.UserId).ToList();
            Hmv.PostWithValue = Ds.GetPostsWithVoteValue(userPosts);
            return View("User", Hmv);
        }

        [HttpGet("Login")]
        public IActionResult login()
        {
            return View("Login");
        }

        [HttpPost("Login")]
        public IActionResult Login(string username, string password)
        {
            User? login = Ds.FindUser(username);
            if (login != null && !Request.Cookies.ContainsKey("ActiveUser") && login.Password == password)
            {
                string userId = login.UserId.ToString();
                Response.Cookies.Append("ActiveUser", userId, new CookieOptions());
                return Redirect("User");
            }
            else if (login != null && Request.Cookies.ContainsKey("ActiveUser") && login.Password == password)
            {
                Response.Cookies.Delete("ActiveUser");
                string userId = login.UserId.ToString();
                Response.Cookies.Append("ActiveUser", userId, new CookieOptions());
                return Redirect("User");
            }
            return RedirectToAction("Index");
        }

        [HttpGet("Logout")]
        public IActionResult LogOut()
        {
            Response.Cookies.Delete("ActiveUser");
            return View("Index", Hmv);
        }

        [HttpGet("Vote/{postId}/{value}")]
        public IActionResult UpVote(int postId, int value)
        {
            if (null != Request.Cookies["ActiveUser"])
            {
                int userId = int.Parse(Request.Cookies["ActiveUser"]);
                Ds.Vote(value, userId, postId);
                return RedirectToAction("Index");
            }
            else return View("Login");
        }


        //Post part of controller
        [HttpGet("Post/{Id}")]
        public IActionResult WievPost(int id)
        {
            Hmv.Post = Ds.GetPost(id);
            return View("Post", Hmv);
        }

        [HttpGet("CreatePost")]
        public IActionResult CreatePost()
        {
            if (null != Request.Cookies["ActiveUser"])
            {
                return View("CreatePost");
            }
            else return View("Login");
        }

        [HttpPost("CreatePost")]
        public IActionResult CreatePost(Post post)
        {
            int userId = int.Parse(Request.Cookies["ActiveUser"]);

            Ds.CreatePost(post, userId);
            return RedirectToAction("Index");
        }

        [HttpGet("EditPost/{Id}")]
        public IActionResult EditPost(int id)
        {
            Hmv.Post = Ds.GetPost(id);
            if (null != Request.Cookies["ActiveUser"] && Hmv.Post.User.UserId == int.Parse(Request.Cookies["ActiveUser"]))
            {
                return View("EditPost", Hmv);
            }
            else 
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost("EditPost/{Id}")]
        public IActionResult EditPost(Post post)
        {
            Ds.EditPost(post);
            return View("Index", Hmv);
        }
    }
}
