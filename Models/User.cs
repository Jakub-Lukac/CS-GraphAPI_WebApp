namespace GraphAPI_WebApp.Models
{
    public class User
    {
        public User(string id, string name, string mail) 
        {
            Id = id;
            DisplayName = name;
            Mail = mail;
        }
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Mail { get; set; }
    }
}
