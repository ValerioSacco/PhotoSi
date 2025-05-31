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
        public async Task Handle_ReturnsGetListProductsResponse_WhenProductsExist()
        {
            // Arrange
            int totalCount = 2;
            int pageNumber = 2;
            int pageSize = 5;
            var category = new Category { Id = Guid.NewGuid(), Name = "Category1" };
            var products = new List<Product>{
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
                    Category = category
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

            for (int i = 0; i < products.Count; i++)
            {
                Assert.Equal(products[i].Id, result.products[i].id);
                Assert.Equal(products[i].Name, result.products[i].name);
                Assert.Equal(products[i].Description, result.products[i].description);
                Assert.Equal(products[i].Price, result.products[i].price);
                Assert.Equal(products[i].ImageUrl, result.products[i].imageUrl);
                Assert.Equal(products[i].Category?.Name, result.products[i].categoryName);
            }
        }
    }
}
