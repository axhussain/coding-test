using MediatR;

namespace ToolsBazaar.Domain.CustomerAggregate.Queries
{
    public sealed record GetTopCustomersQuery : IRequest<IEnumerable<Customer>>
    {
        public GetTopCustomersQuery(DateTime ordersPlacedFrom, DateTime ordersPlacedTo)
        {
            OrdersPlacedFrom = ordersPlacedFrom;
            OrdersPlacedTo = ordersPlacedTo;
        }

        public DateTime OrdersPlacedFrom { get; }
        public DateTime OrdersPlacedTo { get; }
    }
}
