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
    public static readonly object[][] DateRangeMarch2023 =
    {
        new object[] { DateTime.Parse("2023-03-01"), DateTime.Parse("2023-03-30") }
    };

    [Fact]
    public void QueryHandler_GivenNoMatchingOrderDate_ShouldReturnEmptySequence()
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

    [Theory, MemberData(nameof(DateRangeMarch2023))]
    public void QueryHandler_GivenDateRange_ShouldOnlyReturnValidCustomersInDescendingOrderOfAmountSpent(DateTime from, DateTime to)
    {
        var customers = new List<Customer>
        {
            new CustomerBuilder()
                .WithId(1)
                .WithName("Customer One")
                .Build(),
            new CustomerBuilder()
                .WithId(2)
                .WithName("Customer Two")
                .Build()
        };
        var customerRepository = Substitute.For<ICustomerRepository>();
        customerRepository.GetAll().Returns(customers);

        var orders = new List<Order>
        {
            // Customer One's orders
            new OrderBuilder()
                .WithId(1)
                .WithCustomer(customers.First())
                .WithDate(DateTime.Parse("1999-01-01"))
                .WithItems(new List<OrderItem>
                {
                    new OrderItem()
                    {
                        Id = 1,
                        Product = new Product() { Price = 10_000m },
                        Quantity = 1
                    }
                })
                .Build(),
            new OrderBuilder()
                .WithId(2)
                .WithCustomer(customers.First())
                .WithDate(DateTime.Parse("2023-03-15"))
                .WithItems(new List<OrderItem>
                {
                    new OrderItem()
                    {
                        Id = 2,
                        Product = new Product() { Price = 10m },
                        Quantity = 1
                    }
                })
                .Build(),

            // Customer Two's orders
             new OrderBuilder()
                .WithId(3)
                .WithCustomer(customers.Skip(1).First())
                .WithDate(DateTime.Parse("2023-03-15"))
                .WithItems(new List<OrderItem>
                {
                    new OrderItem()
                    {
                        Id = 3,
                        Product = new Product() { Price = 20m },
                        Quantity = 1
                    }
                })
                .Build(),
            new OrderBuilder()
                .WithId(4)
                .WithCustomer(customers.Skip(1).First())
                .WithDate(DateTime.Parse("2023-03-16"))
                .WithItems(new List<OrderItem>
                {
                    new OrderItem()
                    {
                        Id = 4,
                        Product = new Product() { Price = 20m },
                        Quantity = 1
                    }
                })
                .Build()
        };
        var ordersRespository = Substitute.For<IOrderRepository>();
        ordersRespository.GetAll().Returns(orders);

        var logger = Substitute.For<ILogger<GetTopCustomersQueryHandler>>();

        var sut = new GetTopCustomersQueryHandler(customerRepository, ordersRespository, logger);
        var query = new GetTopCustomersQuery(from, to);

        var returnedCustomers = sut.Handle(query, CancellationToken.None).Result.ToList();

        returnedCustomers.First().Id.Should().Be(2);
        returnedCustomers.First().Name.Should().Be("Customer Two");
        returnedCustomers.Skip(1).First().Id.Should().Be(1);
        returnedCustomers.Skip(1).First().Name.Should().Be("Customer One");
    }
}