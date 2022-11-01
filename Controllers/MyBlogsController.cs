#nullable disable
using Microsoft.AspNetCore.Mvc;
using KappaPortal.Models;
using KappaPortal.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using KappaPortal.Models.ViewModels;
using KappaPortal.Models.EmailModel;
using Newtonsoft.Json;
using System.Text;

namespace KappaPortal.Controllers
{
    [Authorize]
    public class MyBlogsController : Controller
    {

        private static readonly HttpClient httpClient = new HttpClient();


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBlogService _blogService;  //Needed an empty constructor in BlogServices to make it work. Got help from Intellisense

        public MyBlogsController(IBlogService blogService, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _blogService = blogService;
        }

        public IActionResult Index()
        {
            var Author = _userManager.GetUserAsync(HttpContext.User).Result;
            MyBlogIndexVM authorBlogList = new MyBlogIndexVM()
            {
                UserBlogList = _blogService.UserBlogList(Author),
                NewBlog = new Blog()
            };

            return View(authorBlogList);
        }


        //[HttpGet] //HttpGet is implicated if empty

        //public IActionResult BlogAdd()
        //{
        //    Blog addBlo = new Blog();
        //    return View(addBlo);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlogAdd(/*[Bind("Title,Description,FeaturedImage")]*/ MyBlogIndexVM model)
        {

            model.NewBlog.TimeStamp = DateTime.Now;
            model.NewBlog.Author = await _userManager.GetUserAsync(HttpContext.User);

            _blogService.BlogAdd(model.NewBlog); //Info from Models/Blog.cs

            string authorName = model.NewBlog.Author.FirstName + " " + model.NewBlog.Author.LastName;
                

            EmailConfirmation sendMsg = new EmailConfirmation()
            {
                Name = authorName,
                Email = model.NewBlog.Author.Email,
                Subject = "Welcome to Kappa Blog portal with your blog " + model.NewBlog.Title + ", dated " + model.NewBlog.TimeStamp.ToString("yyyy-MM-dd"),
                EmailPlain = "Dear " + authorName + "." + "Thank you for starting blogging at Kappa. \n\n" +
                //" Your author name:  " + authorName +"."+ "  \n\n" +
                "Your blog called " + model.NewBlog.Title  +
                "was created " + model.NewBlog.TimeStamp.ToString("yyyy-MM-dd") + ". You will receive regular notifications for number of commments every week. \n\n" +
                "We try to do everything we can to meet your expectations. We hope you are satisfied and will continue to order from us in the future. \n\n" +
                "Team Kappa.",

                EmailHtml = "<p>Dear "+authorName +",</p> <p>Thank you for blogging at Kappa.</p>" +
                "</b></p> <p>Your blog called <b>" + model.NewBlog.Title + " </b> was created at " + 
                model.NewBlog.TimeStamp.ToString("yyyy-MM-dd")+ ". You will receive regular notifications for number of commments every week.  </p>" +
                "<p>We try to do everything we can to meet your expectations. We hope you are satisfied and will continue to blogging with us in the future.</p>"+
                "<p>Team Kappa.</p>"
             };

            var statusMessage = await SendConfirmation(sendMsg);
            TempData["EmailStatus"] = statusMessage;

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlogEdit(/*[Bind("Title,Description,FeaturedImage")]*/ MyBlogIndexVM model)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);

            model.NewBlog.Author = _blogService.BlogGetAuthor(model.NewBlog.Id);

            if (currentUser.Id == model.NewBlog.Author.Id)
            {
                await _blogService.BlogUpdate(model.NewBlog); //Info from Models/Blog.cs
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlogDelete(/*[Bind("Title,Description,FeaturedImage")]*/ MyBlogIndexVM model)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);

            model.NewBlog.Author = _blogService.BlogGetAuthor(model.NewBlog.Id);

            if (currentUser.Id == model.NewBlog.Author.Id) //Doesn't work with  AuthorId
            {
                string blogTitle = _blogService.BlogGetTitle(model.NewBlog.Id);

                if (blogTitle == model.NewBlog.Title)
                {
                    _blogService.BlogDelete(model.NewBlog.Id);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SendConfirmation(EmailConfirmation sendMsg)
        {
            //Azure Funtion Link

            var myEmailFunctionUrl = "https://registrationconfirmation.azurewebsites.net/api/EmailToQueue?code=2HMUEW1v1kfwh7CZInEzvBZzhWXTVKs0nyJC-D-8r__XAzFuNV_6_A==";

            //local function link
            var myLocalEmailFunctionUrl = "http://localhost:7071/api/EmailToQueue";

            var functionUrl = myEmailFunctionUrl; //change when mail client is ready
            string statusMsg = "";


            using (var myRequest = new HttpRequestMessage(HttpMethod.Post, functionUrl))

            {

                string jsonString = JsonConvert.SerializeObject(sendMsg);
                StringContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                myRequest.Content = httpContent;
                HttpResponseMessage newResponse = await httpClient
                                                    .SendAsync(myRequest)
                                                    .ConfigureAwait(false);

                if (newResponse.IsSuccessStatusCode)
                {
                    statusMsg = "Your order has been registered. A confirmation has been sent by email to:";
                }
                else
                {
                    statusMsg = "Something went wrong.";
                }
            };
            return statusMsg;
        }


    }
}
