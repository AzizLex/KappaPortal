using Microsoft.AspNetCore.Mvc;
using KappaPortal.Models;
using KappaPortal.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using KappaPortal.Models.ViewModels;
using KappaPortal.Models.AjaxModels;

namespace KappaPortal.Controllers
{
    [Authorize]
    public class MyPostsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPostService _postService;  //Needed an empty constructor in PostServices to make it work. Got help from Intellisense
        private readonly IBlogService _blogService;

        public MyPostsController(IPostService postService, IBlogService blogService, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _postService = postService;
            _blogService = blogService;
        }

        public async Task<IActionResult> Index(int? Id)
        {

            Blog currentBlog = _blogService.BlogGet(Id ?? 0);

            if (currentBlog == null)
            {
                return RedirectToAction("Index", "MyBlogs");
            }
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser.Id != currentBlog.AuthorId)
            {
                return RedirectToAction("Index", "MyBlogs");
            }

            MyPostIndexVM authorPostList = new MyPostIndexVM()
            {
                UserPostList = _postService.UserPostList(currentUser, Id ?? 0),
                NewPost = new Post()
                {
                    BlogId = Id ?? 0
                }
            };

            ViewBag.blogTitle = _blogService.BlogGetTitle(Id ?? 0);

            return View(authorPostList);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostAdd(/*[Bind("Title,Description,FeaturedImage")]*/int? Id, MyPostIndexVM model)
        {

            Blog currentBlog = _blogService.BlogGet(Id ?? 0);

            if (currentBlog == null)
            {
                return RedirectToAction("Index", "MyBlogs");
            }
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser.Id != currentBlog.AuthorId)
            {
                return RedirectToAction("Index", "MyBlogs");
            }

            model.NewPost.TimeStamp = DateTime.Now;
            model.NewPost.BlogId = currentBlog.Id;
            model.NewPost.Views = 0;
            if (model.TagLine != null)
            {
                model.NewPost.Tags = new List<Tag>();
                char[] delimiters = { ' ', ',', '.', ':', '\t', '/', '|' };
                string[] tags = model.TagLine.Split(delimiters);
                foreach (string tag in tags)
                {
                    if (!string.IsNullOrEmpty(tag))
                    {
                        Tag exsTag = _postService.TagGet(tag);
                        if (exsTag != null)
                        {
                            model.NewPost.Tags.Add(exsTag);
                        }
                        else
                        {
                            exsTag = _postService.TagAdd(tag);
                            model.NewPost.Tags.Add(exsTag);
                        }
                    }
                }
            }


            _postService.PostAdd(model.NewPost); //Info from Models/Post.cs

            ViewBag.AddPostSuccessMessage = "Post added successfully.";
            return RedirectToAction(nameof(Index), new { Id = Id });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostEdit(/*[Bind("Title,Description,FeaturedImage")]*/int? Id, MyPostIndexVM model)
        {
            Blog currentBlog = _blogService.BlogGet(Id ?? 0);

            if (currentBlog == null)
            {
                return RedirectToAction("Index", "MyBlogs");
            }
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser.Id == currentBlog.Author.Id)
            {
                model.NewPost.BlogId = Id ?? 0;
                if (model.TagLine != null)
                {
                    model.NewPost.Tags = new List<Tag>();
                    char[] delimiters = { ' ', ',', '.', ':', '\t', '/', '|' };
                    string[] tags = model.TagLine.Split(delimiters);
                    foreach (string tag in tags)
                    {
                        if (!string.IsNullOrEmpty(tag))
                        {
                            Tag exsTag = _postService.TagGet(tag);
                            if (exsTag != null)
                            {
                                model.NewPost.Tags.Add(exsTag);
                            }
                            else
                            {
                                exsTag = _postService.TagAdd(tag);
                                model.NewPost.Tags.Add(exsTag);
                            }
                        }
                    }
                }
                await _postService.PostUpdate(model.NewPost);
            }


            return RedirectToAction(nameof(Index), new { Id = Id });

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostDelete(/*[Bind("Title,Description,FeaturedImage")]*/int? Id, MyPostIndexVM model)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);

            model.NewPost.Blog = _postService.PostGetBlog(model.NewPost.Id);

            if (currentUser.Id == model.NewPost.Blog.AuthorId)
            {
                string confirmation = model.NewPost.Title.ToLower().Trim('\'');

                if (confirmation == "yes")
                {
                    _postService.PostDelete(model.NewPost.Id);
                }
            }

            return RedirectToAction(nameof(Index), new { Id = Id });
        }

        [HttpPost]
        public JsonResult Like([FromBody] LikeObj likeObj)
        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            if (ModelState.IsValid)
            {
                if (likeObj.PoC == "post")
                {
                    var result = _postService.PostLike(currentUser, likeObj);

                    var response = new
                    {
                        Id = result.Id,
                        LikeCount = result.LikeCount,
                        DislikeCount = result.DislikeCount,
                        UserLike = result.UserLike.Like,
                        UserDislike = result.UserLike.Dislike
                    };

                    return Json(response);

                }

            }
            return Json(new { });

        }

        [HttpPost]
        public JsonResult Comment([FromBody] CommentObj commentObj)
        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            if (ModelState.IsValid)
            {
                Comment comment = new Comment()
                {
                    Body = commentObj.Comment,
                    PostId = commentObj.PostId,
                    ToCommentId = (commentObj.CommentId != 0) ? commentObj.CommentId : null,
                    TimeStamp = DateTime.Now,
                    AuthorId = currentUser.Id,
                };
                var result = _postService.CommentAdd(comment);

                Comment response = new Comment()
                {
                    Id = result.Id,
                    PostId=result.PostId,
                    Body = result.Body,
                    TimeStamp = result.TimeStamp,
                    Author = new()
                    {
                        FirstName = result.Author.FirstName,
                        LastName = result.Author.LastName,
                    }
                };

                return Json(response);

            }

            return Json(new { });
        }

        [HttpPost]
        public JsonResult CommentDel([FromBody] CommentObj commentObj)
        {
            ApplicationUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;

            var comment = _postService.CommentGet(commentObj.CommentId);
            var authorId = comment.Post.Blog.AuthorId;

            if (ModelState.IsValid && authorId == currentUser.Id)
            {
                _postService.CommentDel(commentObj.CommentId);
                Boolean response = true;
                return Json(response);
            }

            return Json(new { });
        }
    }
}
