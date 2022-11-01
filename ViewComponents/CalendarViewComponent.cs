using KappaPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace KappaPortal.ViewComponents
{
    [ViewComponent(Name = "Calendar")]
    public class CalendarViewComponent : ViewComponent
    {
        IBlogService _blogService;
        public CalendarViewComponent(IBlogService blogService)
        {
            _blogService=blogService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int? Id)
        {
            var calendarList = _blogService.CalendarList(Id??0);
            ViewBag.BlogId = Id;
            return View("Index", calendarList);
        }
    }
}
