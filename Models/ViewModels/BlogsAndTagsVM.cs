namespace KappaPortal.Models.ViewModels
{
    public class BlogsAndTagsVM
    {
        public IEnumerable<UserBlogVM> BrowseBlogsByView { get; set; }
        public IEnumerable<TagListVM> BrowsePostsByTags { get; set; }




        //https://www.c-sharpcorner.com/UploadFile/ff2f08/multiple-models-in-single-view-in-mvc/
    }
}
