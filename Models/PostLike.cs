using System.ComponentModel.DataAnnotations;

namespace KappaPortal.Models
{
    public class PostLike
    {
        Boolean like;
        Boolean dislike;
        public int Id { get; set; }
        [StringLength(450)]
        public string? AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; } /* UserId(Link to BlogUser)*/
        public int PostId { get; set; }
        public virtual Post Post { get; set; } /* PostId(Link to Post)*/
        public Boolean Like /*Like(Boolean)*/
        {
            get { return like; }
            set
            {
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

        public PostLike()
        {
            like = false;
            dislike = false;
        }
    }
}