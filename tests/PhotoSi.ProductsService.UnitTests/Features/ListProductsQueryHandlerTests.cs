using NSubstitute;
using PhotoSi.ProductsService.Features.ListProducts;
using PhotoSi.ProductsService.Models;
using PhotoSi.ProductsService.Repositories;

namespace PhotoSi.ProductsService.UnitTests.Features
{
    public class ListProductsQueryHandlerTests
    {
        private readonly ListProductsQueryHandler _handler;
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();

        public ListProductsQueryHandlerTests()
        {
            _handler = new ListProductsQueryHandler(_productRepository);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoProductsExist()
        {
            // Arrange
            int totalCount = 0;
            int pageNumber = 1;
            int pageSize = 10;
            _productRepository.CountAsync(Arg.Any<CancellationToken>()).Returns(totalCount);
            _productRepository.ListAllAsync(pageNumber, pageSize, Arg.Any<CancellationToken>())
                .Returns(new List<Product>());

            var query = new ListProductsQuery(pageNumber, pageSize);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(totalCount, result.count);
            Assert.Equal(pageNumber, result.pageNumber);
            Assert.Equal(pageSize, result.pageSize);
            Assert.Empty(result.products);
        }

        [Fact]
        public async Task Handle_ReturnsMappedProducts_WhenProductsExist()
        {
            // Arrange
            int totalCount = 2;
            int pageNumber = 2;
            int pageSize = 5;
            var category = new Category { Id = Guid.NewGuid(), Name = "Category1" };
            var products = new List<Product>
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product1",
                Description = "Desc1",
                Price = 10.5m,
                ImageUrl = "http://img1",
                Category = category
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product2",
                Description = "Desc2",
                Price = 20.0m,
                ImageUrl = "http://img2",
                Category = null // test null category
            }
        };

            _productRepository.CountAsync(Arg.Any<CancellationToken>()).Returns(totalCount);
            _productRepository.ListAllAsync(pageNumber, pageSize, Arg.Any<CancellationToken>())
                .Returns(products);

            var query = new ListProductsQuery(pageNumber, pageSize);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(totalCount, result.count);
            Assert.Equal(pageNumber, result.pageNumber);
            Assert.Equal(pageSize, result.pageSize);
            Assert.Equal(2, result.products.Count);

            Assert.Equal(products[0].Id, result.products[0].id);
            Assert.Equal(products[0].Name, result.products[0].name);
            Assert.Equal(products[0].Description, result.products[0].description);
            Assert.Equal(products[0].Price, result.products[0].price);
            Assert.Equal(products[0].ImageUrl, result.products[0].imageUrl);
            Assert.Equal(category.Name, result.products[0].categoryName);
        }
    }
}
