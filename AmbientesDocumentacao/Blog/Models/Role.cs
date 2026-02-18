using System.Collections.Generic;

namespace Blog.Models
{
    public class Role
    {
        public Role()
        {
            Users = new List<User>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        public IList<User> Users { get; set; }
    }
}