using Lemon.Backend.Entities;

namespace Lemon.Backend.Services;

public interface IUserRepository : IBaseRepository<User>
{
    public Task<User?> GetByEmailAsync(string email);
    public Task<User?> GetByPhoneNumberAsync(string phoneNumber);
    public Task<User?> GetByUserNameAsync(string userName);

    public Task<bool> AddAsync(User user);
}