namespace PhotoSi.ProductsService.Database
{
    public interface IUnitOfWork
    {
        Guid TransactionId { get; }
        Task StartTransaction(CancellationToken cancellationToken = default);
        Task CommitTransaction(CancellationToken cancellationToken = default);
    }


    public class ProductsUnitOfWork : IUnitOfWork
    {
        private Guid _transactionId;
        private readonly ProductsDbContext _dbContext;

        public ProductsUnitOfWork(ProductsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid TransactionId => _transactionId;

        public async Task StartTransaction(CancellationToken cancellationToken = default)
        {
            await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            _transactionId = Guid.NewGuid();
        }

        public async Task CommitTransaction(CancellationToken cancellationToken = default)
        {
            try
            {
                await _dbContext.SaveChangesAsync();

                if (_dbContext.Database.CurrentTransaction != null)
                {
                    await _dbContext.Database.CommitTransactionAsync(cancellationToken);
                }
            }
            catch (Exception)
            {
                if (_dbContext.Database.CurrentTransaction != null)
                {
                    await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
                }
                throw;
            }
            finally
            {
                await _dbContext.DisposeAsync();
            }
        }

    }


}
