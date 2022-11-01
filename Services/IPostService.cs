using KappaPortal.Models;
using KappaPortal.Models.AjaxModels;
using KappaPortal.Models.ViewModels;

namespace KappaPortal.Services
{
    public interface IPostService
    {
        void PostAdd(Post post);
        Post PostGet(int postId);
        IEnumerable<Post> PostListGet(int start, int end);
        IEnumerable<Post> TrendingPostListGet(int start, int end);
        IEnumerable<Post> LikedPostListGet(int start, int end);
        Blog PostGetBlog(int PostId);
        Task<Boolean> PostUpdate(Post newPost);
        Boolean PostDelete(int id);
        IEnumerable<UserPostVM> UserPostList(ApplicationUser user, int? blogId);
        IEnumerable<UserPostVM> UserPostList(ApplicationUser user, int? blogId, int? yId, int? mId);
        IEnumerable<UserPostVM> AuthorPostList(ApplicationUser user, string userName);
        UserPostVM UserPost(ApplicationUser user, int? postId);
        UserPostVM PostLike(ApplicationUser user, LikeObj likeObj);
        Tag TagAdd(string tag);
        Tag TagGet(int? tagId);
        Tag TagGet(string tag);
        IEnumerable<TagListVM> TagList();
        IEnumerable<UserPostVM> TagPostList(ApplicationUser user, int? tg);
        IEnumerable<UserPostVM> AllSearchresults(ApplicationUser user, string search);
        Comment CommentAdd(Comment comment);
        Comment CommentGet(int commentId);
        Boolean CommentDel(int commentId);

    }
}
