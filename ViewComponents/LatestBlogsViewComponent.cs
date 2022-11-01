using KappaPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace KappaPortal.ViewComponents
{
    [ViewComponent(Name = "LatestBlogs")]
    public class LatestBlogsViewComponent:ViewComponent
    {
        private readonly IBlogService _blogService;
        public LatestBlogsViewComponent(IBlogService blogService)
        {
            _blogService = blogService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var doublePost = _blogService.BlogListGet(1, 5);

            return View("Index", doublePost);
        }
    }
}
