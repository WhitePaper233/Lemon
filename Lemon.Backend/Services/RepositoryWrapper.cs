using Lemon.Backend.Entities;

namespace Lemon.Backend.Services;

public class RepositoryWrapper(LemonDbContext lemonDbContext) : IRepositoryWrapper
{
    private LemonDbContext LemonDbContext { get; } = lemonDbContext;

    #region Repositories
    private IUserRepository? _userRepository = null;
    #endregion

    #region Properties
    public IUserRepository UserRepository => _userRepository ??= new UserRepository(LemonDbContext);
    #endregion
}
