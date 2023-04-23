using MediatR;
using Microsoft.Extensions.Logging;
using ToolsBazaar.Domain.CustomerAggregate;
using ToolsBazaar.Domain.CustomerAggregate.Queries;
using ToolsBazaar.Domain.OrderAggregate;

namespace ToolsBazaar.Application.Features.Customers.QueryHandlers
{
    public class GetTopCustomersQueryHandler : IRequestHandler<GetTopCustomersQuery, IEnumerable<Customer>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<GetTopCustomersQueryHandler> _logger;

        public GetTopCustomersQueryHandler(
            ICustomerRepository customerRepository,
            IOrderRepository orderRepository,
            ILogger<GetTopCustomersQueryHandler> logger)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public Task<IEnumerable<Customer>> Handle(GetTopCustomersQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Invoking {nameof(GetTopCustomersQueryHandler)}...");

            var customers = _customerRepository
                .GetAll()
                .AsQueryable();

            var orders = _orderRepository
                .GetAll()
                .AsQueryable()
                .Where(o => o.Date >= query.OrdersPlacedFrom)
                .Where(o => o.Date <= query.OrdersPlacedTo);

            var topCustomers =
                customers
                .Join(
                    orders,
                    c => c.Id,
                    o => o.Customer.Id,
                    (c, o) => new { Customer = c, Order = o })
                .GroupBy(co => co.Order.Customer)
                .Select(g => new
                {
                    Customer = g.Key,
                    TotalSpent = g.Sum(co => co.Order.GrandTotal)
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(5)
                .ToList();

            var result = topCustomers.Select(o => o.Customer);
            return Task.FromResult(result);
        }
    }
}
