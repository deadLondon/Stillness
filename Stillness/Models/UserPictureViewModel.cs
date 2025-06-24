namespace Stillness.Models
{
    public class UserPictureViewModel
    {
        public User User { get; set; }
        public IEnumerable<Pictures> Pictures { get; set; }
    }
}
