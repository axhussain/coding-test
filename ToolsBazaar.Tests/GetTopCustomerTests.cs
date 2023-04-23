using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ToolsBazaar.Application.Features.Customers.QueryHandlers;
using ToolsBazaar.Domain.CustomerAggregate;
using ToolsBazaar.Domain.CustomerAggregate.Queries;
using ToolsBazaar.Domain.OrderAggregate;
using ToolsBazaar.Domain.ProductAggregate;
using ToolsBazaar.Tests.Builders;

namespace ToolsBazaar.Tests;

public class GetTopCustomerTests
{
    [Fact]
    public void QueryHandler_GivenNoMatchingDate_ShouldReturnEmptySequence()
    {
        var customers = new List<Customer>
        {
            new CustomerBuilder()
                .WithId(1)
                .WithName("Customer One")
                .Build()
        };
        var customerRepository = Substitute.For<ICustomerRepository>();
        customerRepository.GetAll().Returns(customers);

        var orders = new List<Order>
        {
            new OrderBuilder()
                .WithId(1)
                .WithCustomer(customers.First())
                .WithDate(DateTime.Parse("1999-01-01"))
                .WithItems(new List<OrderItem> 
                { 
                    new OrderItem()
                    {
                        Id = 1,
                        Product = new Product() { Price = 10.00m }, 
                        Quantity = 2
                    }
                })
                .Build()
        };
        var ordersRespository = Substitute.For<IOrderRepository>();
        ordersRespository.GetAll().Returns(orders);

        var logger = Substitute.For<ILogger<GetTopCustomersQueryHandler>>();

        var sut = new GetTopCustomersQueryHandler(customerRepository, ordersRespository, logger);
        var query = new GetTopCustomersQuery(DateTime.Today, DateTime.Today);

        var returnedCustomers = sut.Handle(query, CancellationToken.None).Result.ToList();
        returnedCustomers.Should().BeEmpty();
    }
}