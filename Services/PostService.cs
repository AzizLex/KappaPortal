using KappaPortal.Data;
using KappaPortal.Models;
using KappaPortal.Models.AjaxModels;
using KappaPortal.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KappaPortal.Services
{
    public class PostService : IPostService
    {
        readonly ApplicationDbContext _db;
        public PostService(ApplicationDbContext context)
        {
            _db = context;
        }
        public Post PostGet(int postId)
        {
            _db.Update(++_db.Posts.Find(postId).Views);
            _db.SaveChanges();

            return _db.Posts.Find(postId);
        }
        public IEnumerable<Post> PostListGet(int start, int end)
        {
            IEnumerable<Post> posts = _db.Posts
                .OrderByDescending(p => p.TimeStamp)
                .Skip(start - 1).
                Take(end - (start - 1))
                .ToList();

            //_db.Database.ExecuteSqlRaw("UPDATE [Posts] SET [Views]=[Views]+1 FROM [Posts] ORDER BY [TimeStamp] OFFSET "+ (start - 1) + " ROWS FETCH NEXT "+ (end - (start - 1)) + " ROWS ONLY; ");
            foreach (var post in posts)
            {
                post.Views++;
                _db.Update(post);

            }
            _db.SaveChanges();
            return posts;
        }
        public IEnumerable<Post> TrendingPostListGet(int start, int end)
        {
            IEnumerable<Post> posts = _db.Posts
                .OrderByDescending(p => p.Views)
                .Skip(start - 1)
                .Take(end - (start - 1))
                .ToList();

            //_db.Database.ExecuteSqlRaw("UPDATE [Posts] SET [Views]=[Views]+1 FROM [Posts] ORDER BY [Views] OFFSET " + (start - 1) + " ROWS FETCH NEXT " + (end - (start - 1)) + " ROWS ONLY; ");
            foreach (var post in posts)
            {
                post.Views++;
                _db.Update(post);

            }
            _db.SaveChanges();
            return posts;
        }
        public IEnumerable<Post> LikedPostListGet(int start, int end)
        {
            IEnumerable<Post> posts = _db.Posts
                   .OrderByDescending(p => p.PostLikes.Where(pl => pl.Like).Count())
                   .Skip(start - 1)
                   .Take(end - (start - 1))
                   .ToList();
            foreach (var post in posts)
            {
                post.Views++;
                _db.Update(post);

            }
            _db.SaveChanges();
            return posts;
        }
        public void PostAdd(Post post)
        {
            _db.Posts.Add(post);
            _db.SaveChanges();
        }
        public Blog PostGetBlog(int PostId)
        {
            return _db.Posts.Find(PostId).Blog;
        }
        public async Task<Boolean> PostUpdate(Post updatePost)
        {
            try
            {
                Post post = _db.Posts.Find(updatePost.Id);
                if (post == null) return false;

                post.Title = updatePost.Title;
                post.Body = updatePost.Body;
                post.FeaturedImage = updatePost.FeaturedImage;
                post.Tags.Clear();
                post.Tags = updatePost.Tags;

                _db.Update(post);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }
        public Boolean PostDelete(int id) //Delete Post with given Id
        {

            var post = _db.Posts.Find(id);
            if (post == null) return false;

            _db.Posts.Remove(post);
            _db.SaveChanges();

            return true;
        }
        public IEnumerable<UserPostVM> UserPostList(ApplicationUser user, int? blogId)
        {
            IEnumerable<Post> posts = _db.Posts
                                    .Where(p => p.BlogId == blogId);
            if (!posts.Any()) { return null; }


            IEnumerable<UserPostVM> userPostList = posts
                                    .Select(p => new UserPostVM()
                                    {
                                        Id = p.Id,
                                        Title = p.Title,
                                        Body = p.Body,
                                        FeaturedImage = p.FeaturedImage,
                                        TimeStamp = p.TimeStamp,
                                        Views = p.Views,
                                        Tags = p.Tags,
                                        TagLine = String.Join(',', p.Tags.Select(t => " " + t.Name).ToArray()).TrimStart(' '),
                                        LikeCount = p.PostLikes.Count(l => l.Like),
                                        DislikeCount = p.PostLikes.Count(l => l.Dislike),
                                        CommentCount = p.Comments.Count,
                                        UserLike = (user == null) ? new PostLike() : p.PostLikes.FirstOrDefault(l => l.AuthorId == user.Id),
                                        Comments = p.Comments.OrderByDescending(c => c.TimeStamp).ToList()
                                    }).OrderByDescending(p => p.TimeStamp).ToList();

            _db.Database.ExecuteSqlRaw("UPDATE [Posts] SET [Views]=[Views]+1 WHERE [BlogId]=" + blogId);
            return userPostList;
        }

        public IEnumerable<UserPostVM> UserPostList(ApplicationUser user, int? blogId, int? yId, int? mId)
        {
            IEnumerable<Post> posts = _db.Posts
                .Where(p => p.BlogId == blogId);
          
            if(yId is not null) 
            { 
                posts = posts.
                Where(p => p.TimeStamp.Year == yId);
            }
            if (mId is not null)
            {
                posts = posts.
                Where(p => p.TimeStamp.Month == mId);
            }

            if (!posts.Any()) { return null; }

           
            IEnumerable<UserPostVM> userPostList = posts
                                    .Select(p => new UserPostVM()
                                    {
                                        Id = p.Id,
                                        Title = p.Title,
                                        Body = p.Body,
                                        FeaturedImage = p.FeaturedImage,
                                        TimeStamp = p.TimeStamp,
                                        Views = p.Views,
                                        Tags = p.Tags,
                                        TagLine = String.Join(',', p.Tags.Select(t => " " + t.Name).ToArray()).TrimStart(' '),
                                        LikeCount = p.PostLikes.Count(l => l.Like),
                                        DislikeCount = p.PostLikes.Count(l => l.Dislike),
                                        CommentCount = p.Comments.Count,
                                        UserLike = (user == null) ? new PostLike() : p.PostLikes.FirstOrDefault(l => l.AuthorId == user.Id),
                                        Comments = p.Comments.OrderByDescending(c => c.TimeStamp).ToList()
                                    }).OrderByDescending(p => p.TimeStamp).ToList();


            string sql = "UPDATE [Posts] SET [Views]=[Views]+1 WHERE [BlogId]=" + blogId;
            if (yId != null) sql += " AND Year([TimeStamp])="+yId;
            if (mId != null) sql += " AND Month([TimeStamp])="+mId;

            _db.Database.ExecuteSqlRaw(sql); 

            return userPostList;
        }

        public UserPostVM UserPost(ApplicationUser user, int? postId)
        {
            UserPostVM userPostList = _db.Posts
                                  .Where(p => p.Id == postId)
                                  .Select(p => new UserPostVM()
                                  {
                                      Id = p.Id,
                                      Title = p.Title,
                                      Body = p.Body,
                                      FeaturedImage = p.FeaturedImage,
                                      TimeStamp = p.TimeStamp,
                                      Views = p.Views,
                                      Tags = p.Tags,
                                      TagLine = String.Join(',', p.Tags.Select(t => " " + t.Name).ToArray()).TrimStart(' '),
                                      LikeCount = p.PostLikes.Count(l => l.Like),
                                      DislikeCount = p.PostLikes.Count(l => l.Dislike),
                                      CommentCount = p.Comments.Count(),
                                      UserLike = (user == null) ? new PostLike() : p.PostLikes.FirstOrDefault(l => l.AuthorId == user.Id),
                                      Comments = p.Comments.OrderByDescending(c => c.TimeStamp).ToList()
                                  }).FirstOrDefault();

            _db.Database.ExecuteSqlRaw("UPDATE [Posts] SET [Views]=[Views]+1 WHERE ([Id]=" + postId + ")");

            return userPostList;
        }
        public UserPostVM PostLike(ApplicationUser user, LikeObj likeObj)
        {
            PostLike postLike = _db.PostLikes.FirstOrDefault(pl => (pl.PostId == likeObj.Id && pl.AuthorId == user.Id));
            if (postLike == null)
            {
                postLike = new PostLike()
                {
                    AuthorId = user.Id,
                    PostId = likeObj.Id,
                };
                if (likeObj.ActType == "like")
                {
                    postLike.Like = true;
                }
                else if (likeObj.ActType == "dislike")
                {
                    postLike.Dislike = true;
                };
                _db.PostLikes.Add(postLike);
                _db.SaveChanges();
            }
            else
            {
                if (likeObj.ActType == "like")
                {
                    postLike.Like = !postLike.Like;
                }
                else if (likeObj.ActType == "dislike")
                {
                    postLike.Dislike = !postLike.Dislike;
                };
                _db.PostLikes.Update(postLike);
                _db.SaveChanges();
            }
            UserPostVM postVM = UserPost(user, postLike.PostId);
            return postVM;
        }
        public Tag TagAdd(string tag)
        {
            Tag newTag = new Tag() { Name = tag };
            _db.Tags.Add(newTag);
            _db.SaveChanges();
            return newTag;
        }
        public Tag TagGet(int? tagId)
        {
            return _db.Tags.Find(tagId);
        }
        public Tag TagGet(string tag)
        {
            return _db.Tags.FirstOrDefault(t => t.Name == tag);
        }
        public IEnumerable<TagListVM> TagList() //Method for "All blogs"
        {
            IEnumerable<TagListVM> tagList = _db.Tags

                .Select(t => new TagListVM()
                {
                    Id = t.Id,

                    Name = t.Name,
                    PostCount = t.Posts.Count()
                }).OrderByDescending(t => t.PostCount);

            return tagList;
        }

        public IEnumerable<UserPostVM> TagPostList(ApplicationUser user, int? tg) // Method for getting posts filtered by tags
        {
            Tag tag = _db.Tags
                   .FirstOrDefault(t => t.Id == tg);
            if(tag is null) { return null; }
            IEnumerable<UserPostVM> postList = tag
                   .Posts
                   .Select(p => new UserPostVM()
                   {
                       Id = p.Id,
                       Title = p.Title,
                       Body = p.Body,
                       FeaturedImage = p.FeaturedImage,
                       TimeStamp = p.TimeStamp,
                       Views = p.Views,
                       Tags = p.Tags,
                       TagLine = String.Join(',', p.Tags.Select(t => " " + t.Name).ToArray()).TrimStart(' '),
                       LikeCount = p.PostLikes.Count(l => l.Like),
                       DislikeCount = p.PostLikes.Count(l => l.Dislike),
                       CommentCount = p.Comments.Count,
                       UserLike = (user == null) ? new PostLike() : p.PostLikes.FirstOrDefault(l => l.AuthorId == user.Id),
                       Comments = p.Comments.OrderByDescending(c => c.TimeStamp).ToList()
                   }).OrderByDescending(p => p.TimeStamp).ToList();

            _db.Database.ExecuteSqlRaw("UPDATE [Posts] SET [Views]=[Views]+1 FROM [Posts] INNER JOIN [PostTag] ON [PostTag].PostsId=[Posts].Id WHERE [PostTag].TagsId=" + tg);

            return postList;
        }

        public Comment CommentAdd(Comment comment)
        {
            _db.Comments.Add(comment);
            _db.SaveChanges();
            return comment;
        }
        public Comment CommentGet(int commentId)
        {
            return _db.Comments.Find(commentId);
        }
        public Boolean CommentDel(int commentId)
        {
            var comment = _db.Comments.Find(commentId);
            if (comment == null) return false;

            _db.Comments.Remove(comment);
            _db.SaveChanges();

            return true;
        }
        public IEnumerable<UserPostVM> AllSearchresults(ApplicationUser user, string search/*, ApplicationUser user*/)
        {
            IEnumerable<UserPostVM> strList = _db.Posts.
                 Where(s => s.Body.Contains(search))
                 .Union(_db.Posts.Where(s => s.Title.Contains(search)))
                 .Union(_db.Posts.Where(t => t.Tags.Any(m => m.Name == search)))
                 .Union(_db.Posts.Where(t => t.Comments.Any(m => m.Body.Contains(search))))
                 .Select(p => new UserPostVM()
                 {
                     Id = p.Id,
                     Title = p.Title,
                     Body = p.Body,
                     FeaturedImage = p.FeaturedImage,
                     TimeStamp = p.TimeStamp,
                     Views = p.Views,
                     Tags = p.Tags,
                     TagLine = String.Join(',', p.Tags.Select(t => " " + t.Name).ToArray()).TrimStart(' '),
                     LikeCount = p.PostLikes.Count(l => l.Like),
                     DislikeCount = p.PostLikes.Count(l => l.Dislike),
                     CommentCount = p.Comments.Count,
                     UserLike = (user == null) ? new PostLike() : p.PostLikes.FirstOrDefault(l => l.AuthorId == user.Id),
                     Comments = p.Comments.OrderByDescending(c => c.TimeStamp).ToList()
                 }).ToList();

            return strList;
        }
        public IEnumerable<UserPostVM> AuthorPostList(ApplicationUser user, string authorUserName)
        {
            IEnumerable<UserPostVM> posts = _db.Users
                .FirstOrDefault(u => u.UserName == authorUserName)
                .Blogs
                .SelectMany(b => b.Posts)
                .Select(p => new UserPostVM()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Body = p.Body,
                    FeaturedImage = p.FeaturedImage,
                    TimeStamp = p.TimeStamp,
                    Views = p.Views,
                    Tags = p.Tags,
                    TagLine = String.Join(',', p.Tags.Select(t => " " + t.Name).ToArray()).TrimStart(' '),
                    LikeCount = p.PostLikes.Count(l => l.Like),
                    DislikeCount = p.PostLikes.Count(l => l.Dislike),
                    CommentCount = p.Comments.Count,
                    UserLike = (user == null) ? new PostLike() : p.PostLikes.FirstOrDefault(l => l.AuthorId == user.Id),
                    Comments = p.Comments.OrderByDescending(c => c.TimeStamp).ToList()
                }).OrderByDescending(p => p.TimeStamp).ToList();

            return posts;
        }
    }
}
