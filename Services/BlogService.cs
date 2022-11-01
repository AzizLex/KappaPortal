using KappaPortal.Data;
using KappaPortal.Models;
using KappaPortal.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KappaPortal.Services
{
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext _db;

        public BlogService(ApplicationDbContext context)
        {
            _db = context;
        }

        public IEnumerable<UserBlogVM> UserBlogList(ApplicationUser author) //List of blogs for the author
        {
            IEnumerable<UserBlogVM> userBlogList = _db.Blogs
                .Where(b => b.AuthorId == author.Id)
                .Select(b => new UserBlogVM()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    FeaturedImage = b.FeaturedImage,
                    TimeStamp = b.TimeStamp,
                    PostViews = b.Posts.Sum(p => p.Views),
                    PostCount = b.Posts.Count()
                }).OrderByDescending(b => b.TimeStamp).ToList();

            return userBlogList;
        }



        public IEnumerable<UserBlogVM> UserBlogList() //Method for "All blogs"
        {
            IEnumerable<UserBlogVM> userBlogList = _db.Blogs

                .Select(b => new UserBlogVM()
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName=b.Author.FirstName+" "+b.Author.LastName,
                    Description = b.Description,
                    FeaturedImage = b.FeaturedImage,
                    TimeStamp = b.TimeStamp,
                    PostViews = b.Posts.Sum(p => p.Views),
                    PostCount = b.Posts.Count()
                }).OrderByDescending(b => b.PostViews).ToList();

            return userBlogList;
        }
        public IEnumerable<Blog> SearchForBlogs(string searchString)
        {
            IEnumerable<Blog> blogResult = _db.Blogs.Where(s => s.Title!.Contains(searchString)).ToList();
            return blogResult;
        }

        public Boolean BlogAdd(Blog newBlog) // add given newBlog into Blogs table
        {
            _db.Blogs.Add(newBlog);
            if (_db.SaveChanges() > 0)
                return true;
            else return false;
        }

        public IEnumerable<Blog> BlogListGet(int start, int end) // return list of blogs ordered by latest added
        {
            var result = _db.Blogs
                .OrderByDescending(m => m.TimeStamp)
                .Skip(start-1)
                .Take(end-(start-1))
                .ToList();
            return result;
        }

        public List<Blog> BlogSearch(string search) // return list of blogs ordered by latest added
        {
            var result = _db.Blogs.Where(m => m.Title.Contains(search)).ToList();
            return result;
        }

        public Blog BlogGet(int? blogId) // return Blog with given Id
        {
            if (blogId == null) { return null; }
            return _db.Blogs.Find(blogId);
        }

        public Boolean BlogDelete(int? blogId) //Delete Blog with given Id
        {
            if(blogId == null) { return false; }
            var blog = _db.Blogs.Find(blogId);
            if (blog == null) return false;

            _db.Blogs.Remove(blog);
            _db.SaveChanges();

            return true;

        }
        public ApplicationUser BlogGetAuthor(int? blogId)
        {
            
            Blog blog = _db.Blogs.Find(blogId);
            if (blog == null) return null;
            return blog.Author;
        }

        public string BlogGetTitle(int? blogId)
        {
            Blog blog = _db.Blogs.Find(blogId);
            if (blog == null) return null;
            return blog.Title;
        }


        public async Task<Boolean> BlogUpdate(Blog newBlog) // update/change Movie to newMovie
        {
            try
            {
                Blog blog = _db.Blogs.Find(newBlog.Id);
                if (blog == null) return false;

                blog.Title = newBlog.Title;
                blog.Description = newBlog.Description;
                blog.FeaturedImage = newBlog.FeaturedImage;

                _db.Update(blog);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }
        public IEnumerable<CalendarVM> CalendarList(int? blogId)
        {
            Blog blog = _db.Blogs.Find(blogId);
            if (blog==null) return null;


            IEnumerable<CalendarVM> list = blog
                .Posts
                .Select(p => new
                {
                    Year = p.TimeStamp.Year,
                    Month = p.TimeStamp.Month,
                })
                .GroupBy(
                y => y.Year,
                y => y.Month,
                (key, g) => new CalendarVM()
                {
                    Year = key,
                    Months = g.Distinct().OrderBy(m => m).ToList(),
                }).OrderByDescending(y => y.Year)
                .ToList();
            return list;
        }
            public IEnumerable<UserBlogVM> SearchInBlogs(string search)
        {

            IEnumerable<UserBlogVM> blList = _db.Blogs
                .Where(b => b.Title.Contains(search))
                .Union(_db.Blogs.Where(a => a.Author.FirstName.Contains(search)))
                .Union(_db.Blogs.Where(a => a.Author.LastName.Contains(search)))
                .Select(b => new UserBlogVM()
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author.FirstName + " " + b.Author.LastName,
                    Description = b.Description,
                    FeaturedImage = b.FeaturedImage,
                    TimeStamp = b.TimeStamp,
                    PostViews = b.Posts.Sum(p => p.Views),
                    PostCount = b.Posts.Count()
                }).OrderByDescending(b => b.PostViews).ToList();

            return blList;
        }

    }
}
