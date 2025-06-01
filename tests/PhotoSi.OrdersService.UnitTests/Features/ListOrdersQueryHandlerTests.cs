using NSubstitute;
using PhotoSi.OrdersService.Features.ListOrders;
using PhotoSi.OrdersService.Models;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.ProductsService.Features.ListProducts;

namespace PhotoSi.OrdersService.UnitTests.Features
{
    public class ListOrdersQueryHandlerTests
    {
        private readonly ListOrdersQueryHandler _handler;
        private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();

        public ListOrdersQueryHandlerTests()
        {
            _handler = new ListOrdersQueryHandler(_orderRepository);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoOrdersExist()
        {
            // Arrange
            int totalCount = 0;
            int pageNumber = 1;
            int pageSize = 10;
            var query = new ListOrdersQuery(pageNumber, pageSize);
            var cancellationToken = CancellationToken.None;

            _orderRepository.CountAsync(cancellationToken).Returns(0);
            _orderRepository.ListAllAsync(query.pageNumber, query.pageSize, cancellationToken)
                .Returns(new List<Order>());

            // Act
            var result = await _handler.Handle(query, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(totalCount, result.count);
            Assert.Equal(pageNumber, result.pageNumber);
            Assert.Equal(pageSize, result.pageSize);
            Assert.Empty(result.orders);
        }

        [Fact]
        public async Task Handle_ReturnsListOrdersResponse_WhenOrdersExist()
        {
            // Arrange
            int totalCount = 1;
            int pageNumber = 2;
            int pageSize = 5;
            var query = new ListOrdersQuery(pageNumber, pageSize);
            var cancellationToken = CancellationToken.None;

            var user = new User
            {
                FirstName = "Mario",
                LastName = "Bianchi",
                Address = new ShipmentAddress
                {
                    Street = "Via Dante 5",
                    City = "City",
                    Country = "Country",
                    PostalCode = "12345"
                }
            };

            var product = new Product
            {
                Name = "Product1",
                Description = "Desc",
                Price = 10,
                CategoryName = "Cat"
            };

            var orderLine = new OrderLine
            {
                Id = Guid.NewGuid(),
                Quantity = 2,
                Notes = "Note",
                Product = product
            };

            var order = new Order
            {
                Id = Guid.NewGuid(),
                Currency = "EUR",
                User = user,
                OrderLines = new List<OrderLine> { orderLine }
            };

            _orderRepository.CountAsync(cancellationToken).Returns(1);
            _orderRepository.ListAllAsync(query.pageNumber, query.pageSize, cancellationToken)
                .Returns(new List<Order> { order });

            // Act
            var result = await _handler.Handle(query, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(totalCount, result.count);
            Assert.Equal(pageNumber, result.pageNumber);
            Assert.Equal(pageSize, result.pageSize);
            Assert.Single(result.orders);
            Assert.Equal(order.Id, result.orders[0].id);
            Assert.Equal(order.Currency, result.orders[0].currency);
            Assert.Equal(20, result.orders[0].totalAmount);
            Assert.Equal(order.User.FirstName, result.orders[0].user.firstName);
            Assert.Equal(order.User.LastName, result.orders[0].user.lastName);
            Assert.Equal(order.User.Address.Street, result.orders[0].user.shipmentAddress.street);
            Assert.Single(result.orders[0].orderLines);
            Assert.Equal(orderLine.Id, result.orders[0].orderLines[0].orderLineId);
            Assert.Equal(2, result.orders[0].orderLines[0].quantity);
        }

    }
}
