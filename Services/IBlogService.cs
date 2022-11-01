using KappaPortal.Models;
using KappaPortal.Models.ViewModels;

namespace KappaPortal.Services
{
    public interface IBlogService
    {
        ApplicationUser BlogGetAuthor(int? blogId);
        string BlogGetTitle(int? blogId);
        Boolean BlogDelete(int? blogId);
        Task<Boolean> BlogUpdate(Blog newBlog);
        Boolean BlogAdd(Blog blog);
        Blog BlogGet(int? blogId);
        IEnumerable<Blog> BlogListGet(int start, int end);
        IEnumerable<UserBlogVM> UserBlogList(ApplicationUser author);
        IEnumerable<UserBlogVM> UserBlogList(); //Part of thisinterface used for dependency injection for BlogService to publish AllBlogs
        IEnumerable<UserBlogVM> SearchInBlogs(string search);
        IEnumerable<CalendarVM> CalendarList(int? blogId);
    }
}
