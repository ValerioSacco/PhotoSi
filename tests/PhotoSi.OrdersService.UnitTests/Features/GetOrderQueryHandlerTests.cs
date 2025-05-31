using NSubstitute;
using PhotoSi.OrdersService.Features.GetOrder;
using PhotoSi.OrdersService.Models;
using PhotoSi.OrdersService.Repositories;
using PhotoSi.Shared.Exceptions;

namespace PhotoSi.OrdersService.UnitTests.Features
{
    public class GetOrderQueryHandlerTests
    {
        private readonly GetOrderQueryHandler _handler;
        private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();

        public GetOrderQueryHandlerTests()
        {
            _handler = new GetOrderQueryHandler(_orderRepository);
        }

        private static GetOrderQuery CreateQuery(Guid id)
        {
            return new GetOrderQuery(id);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenOrderDoesNotExist()
        {
            // Arrange
            var query = CreateQuery(Guid.NewGuid());
            _orderRepository.GetByIdAsync(query.id, Arg.Any<CancellationToken>())
                .Returns((Order?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ReturnsGetOrderResponse_WhenOrderExists()
        {
            // Arrange
            var query = CreateQuery(Guid.NewGuid());
            var userAddress = new ShipmentAddress
            {
                Street = "123 Main St",
                City = "Testville",
                Country = "Testland",
                PostalCode = "12345"
            };
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Address = userAddress
            };
            var orderLines = new List<OrderLine>
        {
            new OrderLine
            {
                Quantity = 2,
                Product = new Product
                {
                    Name = "Product1",
                    Description = "Desc1",
                    Price = 10.0m,
                    CategoryName = "Cat1"
                }
            },
            new OrderLine
            {
                Quantity = 1,
                Product = new Product
                {
                    Name = "Product2",
                    Description = "Desc2",
                    Price = 20.0m,
                    CategoryName = "Cat2"
                }
            }
        };
            var order = new Order
            {
                Id = query.id,
                Currency = "EUR",
                User = user,
                OrderLines = orderLines
            };

            _orderRepository.GetByIdAsync(query.id, Arg.Any<CancellationToken>())
                .Returns(order);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.Id, result.id);
            Assert.Equal(order.Currency, result.currency);
            Assert.Equal(2 * 10.0m + 1 * 20.0m, result.totalAmount);
            Assert.NotNull(result.user);
            Assert.Equal(user.FirstName, result.user.firstName);
            Assert.Equal(user.LastName, result.user.lastName);
            Assert.NotNull(result.user.shipmentAddress);
            Assert.Equal(userAddress.Street, result.user.shipmentAddress.street);
            Assert.Equal(userAddress.City, result.user.shipmentAddress.city);
            Assert.Equal(userAddress.Country, result.user.shipmentAddress.country);
            Assert.Equal(userAddress.PostalCode, result.user.shipmentAddress.postalCode);

            Assert.Equal(2, result.orderLines.Count);

            Assert.Equal(2, result.orderLines[0].quantity);
            Assert.Equal("Product1", result.orderLines[0].product.name);
            Assert.Equal("Desc1", result.orderLines[0].product.description);
            Assert.Equal(10.0m, result.orderLines[0].product.price);
            Assert.Equal("Cat1", result.orderLines[0].product.categoryName);

            Assert.Equal(1, result.orderLines[1].quantity);
            Assert.Equal("Product2", result.orderLines[1].product.name);
            Assert.Equal("Desc2", result.orderLines[1].product.description);
            Assert.Equal(20.0m, result.orderLines[1].product.price);
            Assert.Equal("Cat2", result.orderLines[1].product.categoryName);
        }
    }
}
