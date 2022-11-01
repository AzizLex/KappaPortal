using System.ComponentModel.DataAnnotations;

namespace KappaPortal.Models
{
    public class Comment
    {
        public int Id { get; set; }   /*Id(Integer - Auto inc)*/

        [Required]    
        [Display(Name ="Write your comment")]
        public string Body { get; set; }    /*Body(String - maybe JSON)*/

        public DateTime TimeStamp { get; set; } /*Timestamp / created date(DateTime)*/

        [Display(Name = "AuthorId")]
        [StringLength(450)]
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public int PostId { get; set; } 
        public virtual Post Post  { get; set; }/*(Link to Blogs) one to many*/
        public int? ToCommentId { get; set; }
        public virtual Comment ToComment { get; set; } /* saved as CommentId(Link to Comment) in the database*/
        public virtual ICollection<CommentLike> CommentLikes { get; set; }
        public Comment() /*class constructor that should be the same name as the  class*/

        {

        }
    }
}