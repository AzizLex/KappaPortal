namespace KappaPortal.Models.ViewModels
{
    public class MyBlogIndexVM
    {
        public IEnumerable<UserBlogVM> UserBlogList { get; set; }
        public Blog NewBlog { get; set; }
    }
}
