using System.ComponentModel.DataAnnotations;

namespace KappaPortal.Models.ViewModels
{
    public class UserPostVM
    {
        public int Id { get; set; }
        [Display(Name = "Post title")]
        public string Title { get; set; }
        public string? Body { get; set; }
        public string FeaturedImage { get; set; }
        [Display(Name = "Created")]
        public DateTime TimeStamp { get; set; }
        [Display(Name = "Views")]
        public int Views{get; set; }

        public IEnumerable<Tag> Tags{ get; set; }
        public string TagLine { get; set; } 

        public int LikeCount { get; set; } 

        public int DislikeCount { get; set; } 
        public PostLike UserLike { get; set; }
        public int CommentCount { get; set; } 
        public ICollection<Comment> Comments { get; set; }


        public UserPostVM()
        {

        }

    }
}
