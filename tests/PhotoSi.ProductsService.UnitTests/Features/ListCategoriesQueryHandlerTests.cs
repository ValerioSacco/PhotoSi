using NSubstitute;
using PhotoSi.ProductsService.Features.ListCategories;
using PhotoSi.ProductsService.Models;
using PhotoSi.ProductsService.Repositories;

namespace PhotoSi.ProductsService.UnitTests.Features
{
    public class ListCategoriesQueryHandlerTests
    {
        private readonly ListCategoriesQueryHandler _handler;
        private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();

        public ListCategoriesQueryHandlerTests()
        {
            _handler = new ListCategoriesQueryHandler(_categoryRepository);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoCategoriesExist()
        {
            // Arrange
            _categoryRepository.ListAllAsync(Arg.Any<CancellationToken>())
                .Returns(new List<Category>());
            var query = new ListCategoriesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.categories);
        }

        [Fact]
        public async Task Handle_ReturnsMappedCategories_WhenCategoriesExist()
        {
            // Arrange
            var categories = new List<Category>
        {
            new Category { Id = Guid.NewGuid(), Name = "Cat1", Description = "Desc1" },
            new Category { Id = Guid.NewGuid(), Name = "Cat2", Description = "Desc2" }
        };

            _categoryRepository.ListAllAsync(Arg.Any<CancellationToken>())
                .Returns(categories);
            var query = new ListCategoriesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.categories.Count);
        }
    }
}
