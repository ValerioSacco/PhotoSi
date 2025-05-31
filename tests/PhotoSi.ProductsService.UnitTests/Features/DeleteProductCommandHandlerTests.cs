using MassTransit;
using NSubstitute;
using PhotoSi.ProductsService.Features.DeleteProduct;
using PhotoSi.ProductsService.Models;
using PhotoSi.ProductsService.Repositories;
using PhotoSi.Shared.Events;
using PhotoSi.Shared.Exceptions;

namespace PhotoSi.ProductsService.UnitTests.Features
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly DeleteProductCommandHandler _handler;
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly IPublishEndpoint _publishEndpoint = Substitute.For<IPublishEndpoint>();

        public DeleteProductCommandHandlerTests()
        {
            _handler = new DeleteProductCommandHandler(_productRepository, _publishEndpoint);
        }

        private static DeleteProductCommand CreateValidCommand(Guid id)
        {
            return new DeleteProductCommand(id);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            _productRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns((Product?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenProductDeletionFails()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var product = new Product { Id = command.id, Name = "Test" };
            _productRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns(product);

            _productRepository.Delete(product).Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_RemovesProduct_WhenProductDeletedSuccessfully()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var product = new Product { Id = command.id, Name = "Test" };
            _productRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns(product);

            _productRepository.Delete(product).Returns(true);
            _productRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _productRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }


        [Fact]
        public async Task Handle_PublishesEvent_WhenProductDeletedSuccessfully()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());
            var product = new Product { Id = command.id, Name = "Test" };
            _productRepository.GetByIdAsync(command.id, Arg.Any<CancellationToken>())
                .Returns(product);

            _productRepository.Delete(product).Returns(true);
            _productRepository.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _publishEndpoint.Received(1).Publish(
                Arg.Is<ProductDeletedEvent>(e => e.id == product.Id),
                Arg.Any<CancellationToken>()
            );
        }
    }
}
