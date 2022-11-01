using System.ComponentModel.DataAnnotations;

namespace KappaPortal.Models
{
    public class CommentLike

    {
        Boolean like;
        Boolean dislike;
        public int Id { get; set; }

        [Display(Name = "AuthorId")]
        [StringLength(450)]
        public string? AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; } /* UserId(Link to BlogUser)*/
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }/* CommentId(Link to Post)*/
        public Boolean Like /*Like(Boolean)*/
        {
            get { return like; }
            set { 
                if (value && dislike)
                {
                    dislike = false;
                }
                like = value;
                } 
        }
        public Boolean Dislike /*Dislike(Boolean)*/
        {
            get { return dislike; }
            set
            {
                if (value && like)
                {
                    like = false;
                }
                dislike = value;
            }
        }
        public CommentLike()
        {
            like = false;
            dislike = false;
        }
    }
}