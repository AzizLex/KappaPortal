namespace KappaPortal.Models.EmailModel
{
    public class EmailConfirmation
    {



        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string EmailPlain { get; set; }
        public string EmailHtml { get; set; }

        public EmailConfirmation()
        {

        }


    }
}
