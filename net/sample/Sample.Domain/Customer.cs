using Codeed.Framework.Domain;

namespace Sample.Domain
{
    public class Customer : Entity, IAggregateRoot
    {
        protected Customer()
        {

        }

        public Customer(string code, string description, string identification)
        {
            Code = code;
            Description = description;
            Identification = identification;
        }

        public string Code { get; private set; }

        public string Description { get; private set; }

        public string Identification { get; private set; }

        public void ChangeCode(string code)
        {
            Code = code;
        }

        public void ChangeDescription(string description)
        {
            Description = description;
        }

        public void ChangeIdentification(string identification)
        {
            Identification = identification;
        }
    }
}
