using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.MVC4.Data
{
    public class Comment
    {
        public Comment()
        {
            this.DateCreated = DateTime.Now;
            this.Status = CommentStatus.PENDING;
        }
        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public int Rating { get; set; }
        public int LikeNumber { get; set; }
        public CommentStatus Status { get; set; }
        public bool Gender { get; set; }
        [MaxLength(100)]
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public Comment Parent { get; set; }
        public int? UserID { get; set; }
        [ForeignKey("UserID")]
        public UserProfile User { get; set; }

        [NotMapped]
        public bool IsPending
        {
            get { return Status.HasFlag(CommentStatus.PENDING); }
            set { Status = value ? Status | CommentStatus.PENDING : Status & ~CommentStatus.PENDING; }
        }
        [NotMapped]
        public bool IsActive
        {
            get { return Status.HasFlag(CommentStatus.ACTIVE); }
            set { Status = value ? Status | CommentStatus.ACTIVE : Status & ~CommentStatus.ACTIVE; }
        }
        public static CommentResult ViewByArticle(int articleId, int pageIndex, int pageSize)
        {
            using (DataContext db = new DataContext())
            {
                var skip = (pageIndex - 1) * pageSize;
                var lst = db.Comments.Where(c => c.ArticleId == articleId && (c.Status & CommentStatus.ACTIVE) > 0).OrderByDescending(c => c.DateCreated).Skip(skip).Take(pageSize).ToList();
                var count = db.Comments.Count(c => c.ArticleId == articleId && (c.Status & CommentStatus.ACTIVE) > 0);
                return new CommentResult { ItemCount = count, List = lst, PageIndex = pageIndex, PageSize = pageSize };
            }
        }
    }
    public class CommentResult
    {
        public int ItemCount { get; set; }
        public List<Comment> List { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    [Flags]
    public enum CommentStatus
    {
        PENDING = 1 << 1,
        ACTIVE = 1 <<2,

        ALL = ~0
    }
}
