using Codeed.Framework.Domain;

namespace Codeed.Framework.Identity.Domain
{
    public class User : Entity, IAggregateRoot
    {
        protected User()
        {

        }

        public User(string uid, string email, string name)
        {
            Uid = uid;
            Email = email;
            Name = name;
        }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public string Uid { get; private set; }

        public string? ImageUrl { get; private set; }

        public void ChangeName(string name)
        {
            Name = name;
        }

        public void ChangeImage(string imageUrl)
        {
            ImageUrl = imageUrl;
        }

        public void ChangeEmail(string email)
        {
            Email = email;
        }
    }
}
