using FinanceProjectDLL;

namespace FinanceProjectDBDLL.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int Id);
        Task AddAsync(User user);
        void Update(User user);
        void Remove(User user);
    }
}