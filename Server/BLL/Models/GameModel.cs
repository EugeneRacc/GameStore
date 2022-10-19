namespace BLL.Models
{
    public class GameModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ICollection<Guid>? GenreIds { get; set; }
        public ICollection<Guid>? ImageIds { get; set; }
    }
}
