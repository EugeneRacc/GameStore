namespace BLL.Models
{
    public class CommentModel
    {
        public Guid? Id { get; set; }
        public string Body { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? GameId { get; set; }
        public Guid UserId { get; set; }
        public Guid? ReplyId { get; set; }
        public ICollection<Guid>? ChildComments { get; set; }
    }
}
