using DatabaseBusinessDLL.Interfaces;
using DatabaseBusinessDLL.Repos;
using FinanceProjectDLL;

namespace DatabaseBusinessDLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        public IUserRepository Users { get; }
        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var output = await SaveChangesAsync();
                Console.WriteLine("DbChanges: " + output);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                Console.WriteLine("Rollback");
                throw;
            }
        }

        public async void Add<T>(T entity) where T : class
        {
            switch (entity)
            {
                case User:
                    var newUser = entity as User ?? throw new Exception("Entity is not a User");
                    await Users.AddAsync(newUser);
                    break;

                default:
                    break;
            }
        }

        public void Remove<T>(T entity) where T : class
        {
            switch (entity)
            {
                case User:
                    var toRemoveUser = entity as User ?? throw new Exception("Entitty is not a User");
                    Users.Remove(toRemoveUser);
                    break;

                default:
                    break;
            }
        }

        public void Update<T>(T entity) where T : class
        {
            switch (entity)
            {
                case User:
                    var toUpdateUser = entity as User ?? throw new Exception("Entity is not a User");
                    Users.Update(toUpdateUser);
                    break;

                default:
                    break;
            }
        }
    }
}