using Lemon.Backend.Entities;

namespace Lemon.Backend.Services;

public class UserRepository(LemonDbContext dbCtx) : BaseRepository<User>(dbCtx), IUserRepository
{
    public async Task<bool> AddAsync(User user)
    {
        // Check if the user already exists
        var result = await GetByConditionAsync(u =>
            (user.Email != null && u.Email == user.Email) ||
            (user.PhoneNumber != null && u.PhoneNumber == user.PhoneNumber)
        );
        if (result.Any())
        {
            return false;
        }

        Create(user);
        return await SaveAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return (await GetByConditionAsync(user => user.Email == email)).FirstOrDefault();
    }

    public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return (await GetByConditionAsync(user => user.PhoneNumber == phoneNumber)).FirstOrDefault();
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return (await GetByConditionAsync(user => user.UserName == userName)).FirstOrDefault();
    }
}