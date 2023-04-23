using FluentAssertions;
using NSubstitute;
using ToolsBazaar.Domain.OrderAggregate;
using ToolsBazaar.Domain.ProductAggregate;
using ToolsBazaar.Tests.Builders;

namespace ToolsBazaar.Tests
{
    public class OrderTests
    {
        [Fact]
        public void OrderGrandTotal_GivenItemWithPrice10AndQty2_ShouldReturn20()
        {
            var orders = new List<Order>
            {
                new OrderBuilder()
                    .WithId(1)
                    .WithItems(new List<OrderItem>
                    {
                        new OrderItem()
                        {
                            Id = 1,
                            Product = new Product() { Price = 10m },
                            Quantity = 2
                        }
                    })
                    .Build()
            };
            var ordersRespository = Substitute.For<IOrderRepository>();
            ordersRespository.GetAll().Returns(orders);

            var actualOrder = ordersRespository.GetAll().First();

            actualOrder.GrandTotal.Should().Be(20m);
        }

        [Fact]
        public void OrderGrandTotal_Given2xItemsWithPrice15AndQty1_ShouldReturn30()
        {
            var orders = new List<Order>
            {
                new OrderBuilder()
                    .WithId(1)
                    .WithItems(new List<OrderItem>
                    {
                        new OrderItem()
                        {
                            Id = 1,
                            Product = new Product() { Price = 15m },
                            Quantity = 1
                        },
                        new OrderItem()
                        {
                            Id = 2,
                            Product = new Product() { Price = 15m },
                            Quantity = 1
                        }
                    })
                    .Build()
            };
            var ordersRespository = Substitute.For<IOrderRepository>();
            ordersRespository.GetAll().Returns(orders);

            var actualOrder = ordersRespository.GetAll().First();

            actualOrder.GrandTotal.Should().Be(30m);
        }
    }
}
