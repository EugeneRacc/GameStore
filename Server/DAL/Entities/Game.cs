namespace DAL.Entities
{
    public class Game : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ICollection<GameGenre> GameGenres { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<GameImage> GameImages { get; set; }
    }
}
