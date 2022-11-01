using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KappaPortal.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [PersonalData]
        [StringLength(50)]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [PersonalData]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
        public virtual ICollection<CommentLike> CommentLikes { get; set; }
        //public virtual ICollection<Blog> FollowBlogs { get; set; }
        public ApplicationUser()
        {


        }
    }
}
