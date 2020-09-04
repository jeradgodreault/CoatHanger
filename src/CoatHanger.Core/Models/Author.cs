using System;

namespace CoatHanger.Core.Models
{
    public class Author
    {
        public string UserName { get; set; }
        public string JobTitle { get; set; }

        public Author(string username)
        {
            // null guards. 
            if (username == null || username == "") throw new ArgumentException($"The {nameof(username)} property cannot null");

            UserName = username;
        }
    }
}
