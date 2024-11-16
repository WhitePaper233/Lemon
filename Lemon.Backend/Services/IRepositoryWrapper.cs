namespace Lemon.Backend.Services;

public interface IRepositoryWrapper
{
    public IUserRepository UserRepository { get; }
}