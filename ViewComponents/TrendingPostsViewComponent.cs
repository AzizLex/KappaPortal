using KappaPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace KappaPortal.ViewComponents
{
    [ViewComponent(Name = "TrendingPosts")]
    public class TrendingPostsViewComponent : ViewComponent
    {
        private readonly IPostService _postService;
        public TrendingPostsViewComponent(IPostService postService)
        {
            _postService = postService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var doublePost = _postService.TrendingPostListGet(1, 5);

            return View("Index", doublePost);
        }
    }
}
