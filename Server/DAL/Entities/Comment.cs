namespace DAL.Entities
{
    public class Comment : BaseEntity
    {
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public Guid? ReplieId { get; set; }
        public Comment ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; }
    }
}
