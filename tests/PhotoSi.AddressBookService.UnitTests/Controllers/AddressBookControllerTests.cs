using PhotoSi.AddressBookService.Controllers;
using PhotoSi.AddressBookService.Database;
using Microsoft.AspNetCore.Mvc;
using PhotoSi.AddressBookService.Models;

namespace PhotoSi.AddressBookService.UnitTests.Controllers
{
    public class AddressBookControllerTests : IClassFixture<AddressBookDbFixture>
    {
        private readonly AddressBookDbContext _dbContext;

        public AddressBookControllerTests(AddressBookDbFixture fixture)
        {
            _dbContext = fixture.DbContext;
        }

        [Fact]
        public async Task Get_ReturnsAddressResponse_WhenAddressExists()
        {
            // Arrange
            var controller = new AddressBookController(_dbContext);

            // Act
            var result = await controller.Get(
                CancellationToken.None,
                "Italia", "Milano", "20100", "Via Roma"
            );

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var address = Assert.IsType<Address>(okResult.Value);
            Assert.Equal("Italia", address.Country);
            Assert.Equal("Milano", address.City);
            Assert.Equal("20100", address.PostalCode);
            Assert.Equal("Via Roma", address.Street);
        }


        [Fact]
        public async Task List_ReturnsAddresses_WhenAddressesExist()
        {
            // Arrange
            var controller = new AddressBookController(_dbContext);

            // Act
            int pageNumber = 1;
            int pageSize = 10;
            var result = await controller.List(CancellationToken.None, pageNumber, pageSize);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsType<ListAddressesResponse>(okResult.Value);
            Assert.Equal(3, list.totalCount);
            Assert.Equal(pageNumber, list.pageNumber);
            Assert.Equal(pageSize, list.pageSize);
            Assert.Equal(3, list.addresses.Count());
        }
    }
}

