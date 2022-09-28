namespace DAL.Entities
{
    public class Genre : BaseEntity 
    {
        public string Title { get; set; }
        public Guid ParentId { get; set; }
        public Genre MainGenre { get; set; }
        public ICollection<Genre> SubGenres { get; set; }
        public ICollection<GameGenre> GameGenres { get; set; }
    }
}
