using System.ComponentModel.DataAnnotations;

namespace KappaPortal.Models.AjaxModels
{
    public class LikeObj
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string PoC { get; set; }
        [Required]
        public string ActType { get; set; }
    }
}
