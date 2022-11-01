using System.ComponentModel.DataAnnotations;

namespace KappaPortal.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name ="Title")]
        public string Title { get; set; } 

        [Display(Name ="Description")]
        public string? Description { get; set; }

        [Required]
        [Display(Name ="Created")]
        public DateTime TimeStamp { get; set; }

        [Required]
        [Display(Name = "AuthorId")]
        [StringLength(450)]
        public string AuthorId { get; set; }

        [Required]
        [Display(Name = "Author")]
        public virtual ApplicationUser Author { get; set; }

        [Display(Name = "Featured image")]
        public string FeaturedImage { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public Blog() //Should't this also have author?
        {

        }
    }
}
