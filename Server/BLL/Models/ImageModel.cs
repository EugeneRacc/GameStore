namespace BLL.Models
{
    public class ImageModel
    {
        public Guid? Id { get; set; }
        public Guid GameId { get; set; }
        public byte[] Image { get; set; }
    }
}
