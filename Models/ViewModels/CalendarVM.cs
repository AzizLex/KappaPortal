namespace KappaPortal.Models.ViewModels
{
    public class CalendarVM
    {
        public int Year { get; set; }
        public ICollection<int> Months { get; set; }
    }
}
