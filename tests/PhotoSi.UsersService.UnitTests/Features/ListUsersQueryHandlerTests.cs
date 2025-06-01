using NSubstitute;
using PhotoSi.UsersService.Features.ListUsers;
using PhotoSi.UsersService.Models;
using PhotoSi.UsersService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSi.UsersService.UnitTests.Features
{
    public class ListUsersQueryHandlerTests
    {
        private readonly ListUsersQueryHandler _handler;
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

        public ListUsersQueryHandlerTests()
        {
            _handler = new ListUsersQueryHandler(_userRepository);
        }


        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoUsersExist()
        {
            // Arrange
            int totalCount = 0;
            int pageNumber = 1;
            int pageSize = 10;
            _userRepository.CountAsync(Arg.Any<CancellationToken>()).Returns(totalCount);
            _userRepository.ListAllAsync(pageNumber, pageSize, Arg.Any<CancellationToken>())
                .Returns(new List<User>());

            var query = new ListUsersQuery(pageNumber, pageSize);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(totalCount, result.count);
            Assert.Equal(pageNumber, result.pageNumber);
            Assert.Equal(pageSize, result.pageSize);
            Assert.Empty(result.users);
        }


        [Fact]
        public async Task Handle_ReturnsGetListUsersResponse_WhenUsersExist()
        {
            // Arrange
            int totalCount = 2;
            int pageNumber = 2;
            int pageSize = 5;
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Mario",
                    LastName = "Rossi",
                    UserName = "mariorossi",
                    Email = "mariorossi@gmail.com",
                    PhoneNumber = "1234567890",
                    ProfilePictureUrl = "http://pic.url",
                    ShipmentAddress = new ShipmentAddress
                    {
                        Country = "Italia",
                        City = "Roma",
                        Street = "Via Roma",
                        PostalCode = "00100"
                    }
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Giulia",
                    LastName = "Bianchi",
                    UserName = "giuliabianchi",
                    Email = "giuliabianchi@gmail.com",
                    PhoneNumber = "1234567890",
                    ProfilePictureUrl = "http://pic.url",
                    ShipmentAddress = new ShipmentAddress
                    {
                        Country = "Italia",
                        City = "Milano",
                        Street = "Via Milano",
                        PostalCode = "00500"
                    }
                }
            };

            var query = new ListUsersQuery(pageNumber, pageSize);
            var cancellationToken = CancellationToken.None;

            _userRepository.CountAsync(cancellationToken).Returns(totalCount);
            _userRepository.ListAllAsync(pageNumber, pageSize, cancellationToken).Returns(users);

            // Act
            var result = await _handler.Handle(query, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(totalCount, result.count);
            Assert.Equal(2, result.pageNumber);
            Assert.Equal(5, result.pageSize);
            Assert.Equal(2, result.users.Count);

            for (int i = 0; i < users.Count; i++)
            {
                Assert.Equal(users[i].Id, result.users[i].id);
                Assert.Equal(users[i].FirstName, result.users[i].firstname);
                Assert.Equal(users[i].LastName, result.users[i].lastname);
                Assert.Equal(users[i].UserName, result.users[i].username);
                Assert.Equal(users[i].Email, result.users[i].email);
                Assert.Equal(users[i].PhoneNumber, result.users[i].phoneNumber);
                Assert.Equal(users[i].ProfilePictureUrl, result.users[i].profilePictureUrl);

                Assert.NotNull(result.users[i].shipmentAddress);
                Assert.Equal(users[i].ShipmentAddress?.Country ?? string.Empty, result.users[i].shipmentAddress.country);
                Assert.Equal(users[i].ShipmentAddress?.City ?? string.Empty, result.users[i].shipmentAddress.city);
                Assert.Equal(users[i].ShipmentAddress?.Street ?? string.Empty, result.users[i].shipmentAddress.street);
                Assert.Equal(users[i].ShipmentAddress?.PostalCode ?? string.Empty, result.users[i].shipmentAddress.postalCode);
            }
        }

    }
}
