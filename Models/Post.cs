using System.ComponentModel.DataAnnotations;

namespace KappaPortal.Models
{
    public class Post
    {
        public int Id { get; set; }   /*Id(Integer - Auto inc)*/

        [Required]
        [StringLength(256)]
        [Display(Name ="Title")]
        public string Title { get; set; } 
        
        [Display(Name ="Post content")]
        public string Body { get; set; }    /*Body(String - maybe JSON)*/

        [Display(Name = "Created")]
        public DateTime TimeStamp { get; set; } /*Timestamp / created date(DateTime)*/
        public int Views { get; set; } /*number of views*/
        
        [Required]
        public int BlogId { get; set; }

        [Display(Name = "Featured image")]
        public string FeaturedImage { get; set; }
        public virtual Blog Blog { get; set; }/*(Link to Blogs) one to many*/
        public virtual ICollection<Tag> Tags { get; set; }/*(List of Tags) many to many*/
        public virtual ICollection<PostLike> PostLikes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public Post()
        {

        }
    }
}
