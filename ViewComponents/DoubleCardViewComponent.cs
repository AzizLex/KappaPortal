using KappaPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace KappaPortal.ViewComponents
{
    [ViewComponent(Name = "DoubleCard")]
    public class DoubleCardViewComponent : ViewComponent
    {
        private readonly IPostService _postService;
        public DoubleCardViewComponent(IPostService postService)
        {
            _postService=postService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var doublePost = _postService.PostListGet(4, 5);

            return View("Index", doublePost);
        }
    }
}
