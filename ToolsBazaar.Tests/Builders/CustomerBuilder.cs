using ToolsBazaar.Domain.CustomerAggregate;

namespace ToolsBazaar.Tests.Builders
{
    public class CustomerBuilder
    {
        private int _id = TestsHelper.GetRandomInt();
        private string _name = TestsHelper.GetRandomString(10);
        private string _email = TestsHelper.GetRandomEmail();
        private string _address = TestsHelper.GetRandomString(22);

        public CustomerBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public CustomerBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CustomerBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public CustomerBuilder WithAddress(string address)
        {
            _address = address;
            return this;
        }

        public Customer Build()
        {
            return new Customer()
            {
                Id = _id,
                Name = _name,
                Email = _email,
                Address = _address
            };
        }
    }
}
