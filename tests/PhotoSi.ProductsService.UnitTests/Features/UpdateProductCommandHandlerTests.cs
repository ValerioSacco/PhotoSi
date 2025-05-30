using NSubstitute;
using PhotoSi.ProductsService.Features.UpdateProduct;
using PhotoSi.ProductsService.Models;
using PhotoSi.ProductsService.Repositories;
using PhotoSi.Shared.Exceptions;

namespace PhotoSi.ProductsService.UnitTests.Features
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly UpdateProductCommandHandler _handler;
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();

        public UpdateProductCommandHandlerTests()
        {
            _handler = new UpdateProductCommandHandler(_productRepository, _categoryRepository);
        }

        private static UpdateProductCommand CreateCommand(Guid id, Guid categoryId)
        {
            return new UpdateProductCommand(
                id, 
                "Updated Name", 
                "Updated Description", 
                123.45m, 
                "http://updated/image.jpg", 
                categoryId
            );
        }


        [Fact]
        public async Task Handle_ThrowsBusinessRuleException_WhenCategoryDoesNotExist()
        {
            // Arrange
            var command = CreateCommand(Guid.NewGuid(), Guid.NewGuid());
            _categoryRepository.GetByIdAsync(command.categoryId, Arg.Any<CancellationToken>())
                .Returns((Category?)null);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = CreateCommand(Guid.NewGuid(), Guid.NewGuid());
            var category = new Category { Id = command.categoryId, Name = "Cat" };
            _categoryRepository.GetByIdAsync(command.categoryId, Arg.Any<CancellationToken>())
                .Returns(category);

            _productRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns((Product?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenUpdateFails()
        {
            // Arrange
            var command = CreateCommand(Guid.NewGuid(), Guid.NewGuid());
            var category = new Category { Id = command.categoryId, Name = "Cat" };
            var product = new Product { Id = command.id, Name = "Old", Description = "Old", Price = 1, ImageUrl = "old", CategoryId = category.Id };

            _categoryRepository.GetByIdAsync(command.categoryId, Arg.Any<CancellationToken>())
                .Returns(category);

            _productRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns(product);

            _productRepository.Update(product).Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_UpdatesProductAndReturnsId_WhenUpdateSucceeds()
        {
            // Arrange
            var command = CreateCommand(Guid.NewGuid(), Guid.NewGuid());
            var category = new Category { Id = command.categoryId, Name = "Cat" };
            var product = new Product { Id = command.id, Name = "Old", Description = "Old", Price = 1, ImageUrl = "old", CategoryId = category.Id };

            _categoryRepository.GetByIdAsync(command.categoryId, Arg.Any<CancellationToken>())
                .Returns(category);

            _productRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns(product);

            _productRepository.Update(product).Returns(true);
            _productRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(product.Id, result);
            Assert.Equal(command.name, product.Name);
            Assert.Equal(command.description, product.Description);
            Assert.Equal(command.price, product.Price);
            Assert.Equal(command.imageUrl, product.ImageUrl);
            Assert.Equal(command.categoryId, product.CategoryId);
            await _productRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
