using KappaPortal.Models;
using KappaPortal.Models.ViewModels;
using KappaPortal.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KappaPortal.Controllers
{
    public class BlogController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BlogController> _logger;
        private readonly IPostService _postService;
        private readonly IBlogService _blogService;
        public BlogController(ILogger<BlogController> logger, IBlogService blogService, IPostService postService, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _blogService = blogService; 
            _postService = postService;
            _userManager = userManager;
        }
        public IActionResult Index(int? Id)
        {
            

            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            IEnumerable<UserPostVM> postList = _postService.UserPostList(currentUser, Id);
            if (postList is null)
            {
                return RedirectToAction("Index","Home");
            }

            ViewBag.BlogId = Id;
            ViewBag.blogTitle = _blogService.BlogGetTitle(Id);

            return View(postList);
        }
        public IActionResult CalendarPosts(int? Id,int? yId, int? mId)
        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            IEnumerable<UserPostVM> postList = _postService.UserPostList(currentUser, Id, yId,mId);

            if (postList is null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.BlogId = Id;
            ViewBag.blogTitle = _blogService.BlogGetTitle(Id);
            
            return View("Index", postList);
        }
        public IActionResult Post(int? Id)
        {

            UserPostVM post = _postService.UserPost(null, Id);
            if(post == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Single = "show";
            return View(post);
        }
        public IActionResult Author(string Id)
        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            ApplicationUser author=_userManager.Users.FirstOrDefault(x => x.UserName == Id);
            ViewBag.authorName = author.FirstName + " " + author.LastName;
            IEnumerable<UserPostVM> postList = _postService.AuthorPostList(currentUser, Id);

            return View(postList);
        }
    }
}