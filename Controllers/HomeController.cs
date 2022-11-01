
﻿using KappaPortal.Data;
using KappaPortal.Models;
using KappaPortal.Models.ViewModels;
using KappaPortal.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KappaPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;
        private readonly IBlogService _blogService;

        public HomeController(ILogger<HomeController> logger, IBlogService blogService,IPostService postService, UserManager<ApplicationUser> userManager)
        {
            _postService = postService;
            _logger = logger;
            _blogService = blogService;
            _userManager = userManager;
        }

        public IActionResult E404()
        {
            return View();
        }
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AllBlogs()
        {
            BlogsAndTagsVM modelBlogsAndTagsVM = new BlogsAndTagsVM();
            modelBlogsAndTagsVM.BrowseBlogsByView = _blogService.UserBlogList();
            modelBlogsAndTagsVM.BrowsePostsByTags = _postService.TagList();

            return View(modelBlogsAndTagsVM);
        }

        [Route("Home/TagFilteredPosts/{Id?}")]
        public IActionResult TagFilteredPosts(int? Id)
        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            Tag tag = _postService.TagGet(Id);
            if(tag is null) { return RedirectToAction("index"); }

            ViewBag.tagName = tag.Name;

            return View(_postService.TagPostList(currentUser, Id));
        }

        [Route("Home/Searchresults/{Id?}")]
        [HttpPost]
        public IActionResult SearchResultList(String searchString)
        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            Tuple<IEnumerable<UserPostVM>, IEnumerable<UserBlogVM>> returnTup = 
                new Tuple<IEnumerable<UserPostVM>, IEnumerable<UserBlogVM>>(_postService.AllSearchresults(currentUser, searchString), _blogService.SearchInBlogs(searchString));

            //returnTup = (_postService.AllSearchresults(currentUser, searchString), _blogService.BlogSearch(currentUser, searchString));
            return View(returnTup);
        }

        public IActionResult About()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}