using KappaPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace KappaPortal.ViewComponents
{
    [ViewComponent(Name = "LatestPostsCarousel")]
    public class LatestPostsCarouselViewComponent : ViewComponent
    {
        private readonly IPostService _postService;
        public LatestPostsCarouselViewComponent(IPostService postService)
        {
            _postService=postService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var latestPosts = _postService.PostListGet(1,3);
            
            return View("Index", latestPosts);
        }
    }
}
