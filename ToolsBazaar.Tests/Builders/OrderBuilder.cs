using ToolsBazaar.Domain.CustomerAggregate;
using ToolsBazaar.Domain.OrderAggregate;

namespace ToolsBazaar.Tests.Builders
{
    public class OrderBuilder
    {
        private int _id = TestsHelper.GetRandomInt();
        private DateTime _date = DateTime.Today;
        private List<OrderItem> _items = new();
        private Customer _customer = new();

        public OrderBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public OrderBuilder WithDate(DateTime date)
        {
            _date = date;
            return this;
        }

        public OrderBuilder WithCustomer(Customer customer)
        {
            _customer = customer;
            return this;
        }

        public OrderBuilder WithItems(List<OrderItem> items)
        {
            _items = items;
            return this;
        }

        public Order Build()
        {
            return new Order()
            {
                Id = _id,
                Date = _date,
                Customer = _customer,
                Items = _items
            };
        }
    }
}
