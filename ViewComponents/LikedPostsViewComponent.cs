using KappaPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace KappaPortal.ViewComponents
{
    [ViewComponent(Name = "LikedPosts")]
    public class LikedPostsViewComponent:ViewComponent
    {
        private readonly IPostService _postService;
        public LikedPostsViewComponent(IPostService postService)
        {
            _postService = postService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var doublePost = _postService.LikedPostListGet(1, 5);

            return View("Index", doublePost);
        }
    }
}
