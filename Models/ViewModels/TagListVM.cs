using System.ComponentModel.DataAnnotations;

namespace KappaPortal.Models
{
    public class TagListVM
    {
        public int Id { get; set; }   /*Id(Integer - Auto incremented)*/


        public string Name { get; set; }
        public int PostCount{ get; set; }
        public TagListVM()
        {

        }
    }
}
