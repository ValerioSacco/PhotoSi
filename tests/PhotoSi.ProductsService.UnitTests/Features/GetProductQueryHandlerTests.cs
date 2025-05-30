using NSubstitute;
using PhotoSi.ProductsService.Features.GetProduct;
using PhotoSi.ProductsService.Models;
using PhotoSi.ProductsService.Repositories;
using PhotoSi.Shared.Exceptions;

namespace PhotoSi.ProductsService.UnitTests.Features
{
    public class GetProductQueryHandlerTests
    {
        private readonly GetProductQueryHandler _handler;
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();

        public GetProductQueryHandlerTests()
        {
            _handler = new GetProductQueryHandler(_productRepository);
        }

        private static GetProductQuery CreateQuery(Guid id)
        {
            return new GetProductQuery(id);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var query = CreateQuery(Guid.NewGuid());
            _productRepository.GetByIdAsync(query.id, Arg.Any<CancellationToken>())
                .Returns((Product?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ReturnsGetProductResponse_WhenProductExists()
        {
            // Arrange
            var query = CreateQuery(Guid.NewGuid());
            var category = new Category { Id = Guid.NewGuid(), Name = "TestCategory" };
            var product = new Product
            {
                Id = query.id,
                Name = "TestProduct",
                Description = "TestDescription",
                Price = 123.45m,
                ImageUrl = "http://test/image.jpg",
                Category = category
            };

            _productRepository.GetByIdAsync(query.id, Arg.Any<CancellationToken>())
                .Returns(product);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.id);
            Assert.Equal(product.Name, result.name);
            Assert.Equal(product.Description, result.description);
            Assert.Equal(product.Price, result.price);
            Assert.Equal(product.ImageUrl, result.imageUrl);
            Assert.Equal(product.Category.Name, result.categoryName);
        }
    }
}
