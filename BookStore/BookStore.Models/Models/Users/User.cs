using AspNetCore.Identity.MongoDbCore.Models;

namespace BookStore.Models.Models.Users
{
    public class User : MongoIdentityUser<Guid>
    {
        public User()
        {
            
        }

        public User(string userName, string password) : base(userName, password)
        {
            
        }
        public string AuthorId { get; set; } = String.Empty;
    }
}
