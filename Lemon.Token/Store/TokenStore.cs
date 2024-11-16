using System.Collections.Concurrent;
using Lemon.Token.Generator;

namespace Lemon.Token.Store;

public static class TokenStore
{
    private static readonly ConcurrentDictionary<Guid, List<TokenInfo>> _storage = [];
    private static readonly ConcurrentDictionary<Guid, ReaderWriterLockSlim> _locks = [];

    private const int MaxTokenCount = Constants.JWTToken.MaxTokenCount;

    public static ConcurrentDictionary<Guid, List<TokenInfo>> Storage
    {
        get
        {
            return new ConcurrentDictionary<Guid, List<TokenInfo>>(_storage);
        }
    }

    public static bool TryAddToken(Guid userId, string token, DateTime expireTime)
    {
        var tokens = _storage.GetOrAdd(userId, []);
        var tokenInfoLock = _locks.GetOrAdd(userId, new ReaderWriterLockSlim());

        tokenInfoLock.EnterWriteLock();
        try
        {
            // Remove expired tokens
            tokens.RemoveAll(tokenInfo => tokenInfo.ExpireTime < DateTime.Now);

            // Add new token
            tokens.Add(new TokenInfo
            {
                Token = token,
                ExpireTime = expireTime
            });

            // Remove overlimit tokens
            if (tokens.Count > MaxTokenCount)
            {
                var newTokens = from tokenInfo in tokens
                                orderby tokenInfo.ExpireTime
                                select tokenInfo;
                _storage[userId] = newTokens.TakeLast(MaxTokenCount).ToList();
            }
        }
        finally
        {
            tokenInfoLock.ExitWriteLock();
        }

        return true;
    }

    public static bool TryGenerateAndAddToken(Guid userId, string nickName, DateTime expireTime, out string token)
    {
        token = JWTGenerator.GenerateToken(userId, nickName, expireTime);

        return TryAddToken(userId, token, expireTime);
    }

    public static bool TryRemoveToken(Guid userId, string token)
    {
        if (_storage.TryGetValue(userId, out var tokens))
        {
            var tokenInfoLock = _locks.GetOrAdd(userId, new ReaderWriterLockSlim());
            tokenInfoLock.EnterWriteLock();
            try
            {
                var tokenInfoToRemove = tokens.FirstOrDefault(tokenInfo => tokenInfo.Token == token);
                if (tokenInfoToRemove != null)
                {
                    tokens.Remove(tokenInfoToRemove);
                }
            }
            finally
            {
                tokenInfoLock.ExitWriteLock();
            }
        }
        return true;
    }

    public static bool TryRemoveAllTokens(Guid userId)
    {
        if (_storage.TryRemove(userId, out _))
        {
            _locks.TryRemove(userId, out _);
        }
        return true;
    }

    public static bool VerifyToken(Guid userId, string token)
    {
        if (_storage.TryGetValue(userId, out var tokens))
        {
            var tokenInfoLock = _locks.GetOrAdd(userId, new ReaderWriterLockSlim());
            tokenInfoLock.EnterReadLock();
            try
            {
                var tokenInfo = tokens.FirstOrDefault(tokenInfo => tokenInfo.Token == token);
                if (tokenInfo != null && tokenInfo.ExpireTime > DateTime.Now)
                {
                    return true;
                }
            }
            finally
            {
                tokenInfoLock.ExitReadLock();
            }
        }

        return false;
    }
}
