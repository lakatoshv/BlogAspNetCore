namespace Blog.Data.Models
{
    using Blog.Data.Core;
    using Blog.Data.Core.Models;

    public class Setting : Entity
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
