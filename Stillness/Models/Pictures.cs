namespace Stillness.Models
{
    public class Pictures
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Filename { get; set; }
        public string Description { get; set; }
        public string Filepath { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
