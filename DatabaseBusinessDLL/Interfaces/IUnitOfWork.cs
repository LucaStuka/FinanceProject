namespace DatabaseBusinessDLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        void Add<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
    }
}