using System.ComponentModel.DataAnnotations;

namespace KappaPortal.Models
{
    public class Tag
    {
        public int Id { get; set; }   /*Id(Integer - Auto inc)*/

        [Required]
        [StringLength(150)]
        [Display(Name ="Tag")]
        public string Name { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public Tag()
        {

        }
    }
}
