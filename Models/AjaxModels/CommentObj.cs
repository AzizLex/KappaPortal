using System.ComponentModel.DataAnnotations;

namespace KappaPortal.Models.AjaxModels
{
    public class CommentObj
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public int CommentId { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}
