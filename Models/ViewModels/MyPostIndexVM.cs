using System.ComponentModel.DataAnnotations;

namespace KappaPortal.Models.ViewModels
{
    public class MyPostIndexVM
    {
        public IEnumerable<UserPostVM> UserPostList { get; set; }
        public Post NewPost { get; set; }
        [Display(Name = "Tags")]
        public string TagLine { get; set; }
    }
}
