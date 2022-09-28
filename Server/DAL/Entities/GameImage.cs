namespace DAL.Entities
{
    public class GameImage : BaseEntity
    {
        public Guid GameId { get; set; }
        public byte[] Image { get; set; }
        public Game Game { get; set; }
    }
}
