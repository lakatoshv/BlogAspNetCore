namespace Blog.Data.Models
{
    using Blog.Data.Core;

    public class RefreshToken : Entity
    {
        public string Token { get; set; }
        public virtual ApplicationUser User { get; set; }

        public string UserId { get; set; }
    }
}
