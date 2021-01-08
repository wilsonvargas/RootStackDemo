using System;
using System.Collections.Generic;
using System.Text;

namespace RootStack.Demo.Models
{
    public class BackendResponse
    {
        public IEnumerable<User> Results { get; set; }
        public Info Info { get; set; }
    }

    public class User
    {
        public string Gender { get; set; }
        public Name Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Picture Picture { get; set; }
    }

    public class Info
    {
        public string Seed { get; set; }
        public int Results { get; set; }
        public int Page { get; set; }
        public string Version { get; set; }
    }

    public class Picture
    {
        public string Large { get; set; }
        public string Medium { get; set; }
        public string Thumbnail { get; set; }
    }

    public class Name
    {
        public string Title { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
    }
}
