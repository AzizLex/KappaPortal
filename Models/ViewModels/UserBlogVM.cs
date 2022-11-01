using System.ComponentModel.DataAnnotations;

namespace KappaPortal.Models.ViewModels
{
    public class UserBlogVM
    {
        public int Id { get; set; }
        [Display(Name = "Blog title")]
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string? Description { get; set; }
        public string FeaturedImage { get; set; }
        [Display(Name = "Created")]
        public DateTime TimeStamp { get; set; }
        [Display(Name = "Views")]
        public int PostViews{get; set; }
        [Display(Name = "Posts")]
        public int PostCount { get; set; }
        public UserBlogVM()
        {

        }

    }
}
