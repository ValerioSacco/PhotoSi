using NSubstitute;
using PhotoSi.Shared.Exceptions;
using PhotoSi.UsersService.Features.GetUser;
using PhotoSi.UsersService.Models;
using PhotoSi.UsersService.Repositories;

namespace PhotoSi.UsersService.UnitTests.Features
{
    public class GetUserQueryHandlerTests
    {
        private readonly GetUserQueryHandler _handler;
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

        public GetUserQueryHandlerTests()
        {
            _handler = new GetUserQueryHandler(_userRepository);
        }

        private static GetUserQuery CreateQuery(Guid id)
        {
            return new GetUserQuery(id);
        }


        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenUserDoesNotExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns((User?)null);
            var query = CreateQuery(userId);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }


        [Fact]
        public async Task Handle_UserExists_ReturnsGetUserResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                UserName = "testuser",
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                ProfilePictureUrl = "http://example.com/pic.jpg",
                ShipmentAddress = new ShipmentAddress
                {
                    Country = "Country",
                    City = "City",
                    Street = "Street 1",
                    PostalCode = "12345"
                }
            };
            _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
            var query = CreateQuery(userId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.id);
            Assert.Equal("Test", result.firstname);
            Assert.Equal("User", result.lastname);
            Assert.Equal("testuser", result.username);
            Assert.Equal("test@example.com", result.email);
            Assert.Equal("1234567890", result.phoneNumber);
            Assert.Equal("http://example.com/pic.jpg", result.profilePictureUrl);
            Assert.NotNull(result.shipmentAddress);
            Assert.Equal("Country", result.shipmentAddress.country);
            Assert.Equal("City", result.shipmentAddress.city);
            Assert.Equal("Street 1", result.shipmentAddress.street);
            Assert.Equal("12345", result.shipmentAddress.postalCode);
        }

    }
}
