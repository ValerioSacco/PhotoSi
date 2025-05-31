using MassTransit;
using NSubstitute;
using PhotoSi.ProductsService.Features.CreateProduct;
using PhotoSi.ProductsService.Models;
using PhotoSi.ProductsService.Repositories;
using PhotoSi.Shared.Events;
using PhotoSi.Shared.Exceptions;

namespace PhotoSi.ProductsService.UnitTests.Features
{
    public class CreateProductCommandHandlerTests
    {
        private readonly CreateProductCommandHandler _handler;
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
        private readonly IPublishEndpoint _publishEndpoint = Substitute.For<IPublishEndpoint>();

        public CreateProductCommandHandlerTests()
        {
            _handler = new CreateProductCommandHandler(
                _productRepository,
                _categoryRepository,
                _publishEndpoint
            );
        }

        private static CreateProductCommand CreateValidCommand(Guid categoryId)
        {
            return new CreateProductCommand(
                "Test Product",
                "Test Description",
                99.99m,
                "http://test/image.jpg",
                categoryId
            );
        }

        [Fact]
        public async Task Handle_ThrowsBusinessRuleException_WhenCategoryDoesNotExist()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            _categoryRepository.GetByIdAsync(command.categoryId, Arg.Any<CancellationToken>())
                .Returns((Category?)null);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenProductCreationFails()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var category = new Category { Id = command.categoryId, Name = "Poster" };
            _categoryRepository.GetByIdAsync(command.categoryId, Arg.Any<CancellationToken>())
                .Returns(category);

            _productRepository.Create(Arg.Any<Product>()).Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ReturnsProductId_WhenProductCreatedSuccessfully()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var category = new Category { Id = command.categoryId, Name = "Album" };
            _categoryRepository.GetByIdAsync(command.categoryId, Arg.Any<CancellationToken>())
                .Returns(category);

            _productRepository.Create(Arg.Any<Product>()).Returns(true);
            _productRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }


        [Fact]
        public async Task Handle_SavesProduct_WhenProductCreatedSuccessfully()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var category = new Category { Id = command.categoryId, Name = "Album" };
            _categoryRepository.GetByIdAsync(command.categoryId, Arg.Any<CancellationToken>())
                .Returns(category);

            _productRepository.Create(Arg.Any<Product>()).Returns(true);
            _productRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _productRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

        }


        [Fact]
        public async Task Handle_PublishesEvent_WhenProductCreatedSuccessfully()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var category = new Category { Id = command.categoryId, Name = "Album" };
            _categoryRepository.GetByIdAsync(command.categoryId, Arg.Any<CancellationToken>())
                .Returns(category);

            _productRepository.Create(Arg.Any<Product>()).Returns(true);
            _productRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _publishEndpoint.Received(1).Publish(
                Arg.Is<ProductCreatedEvent>(e =>
                    e.id == result &&
                    e.name == command.name &&
                    e.description == command.description &&
                    e.price == command.price &&
                    e.categoryName == category.Name
                ), Arg.Any<CancellationToken>());

        }
    }
}

