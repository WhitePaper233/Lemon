using Newtonsoft.Json;

namespace Lemon.Token.Test;

[TestClass]
public sealed class DefaultTest
{
    private static void PrintStorage()
    {
        Console.WriteLine(
            JsonConvert.SerializeObject(Store.TokenStore.Storage, Formatting.Indented)
        );
    }

    [TestMethod]
    public void TestAddToken()
    {
        var id = Guid.NewGuid();
        Assert.IsTrue(Store.TokenStore.TryAddToken(id, "token", DateTime.Now.AddDays(1)));
        PrintStorage();
        Assert.IsTrue(Store.TokenStore.VerifyToken(id, "token"));
    }

    [TestMethod]
    public void TestRemoveToken()
    {
        var id = Guid.NewGuid();
        Assert.IsTrue(Store.TokenStore.TryAddToken(id, "token", DateTime.Now.AddDays(1)));
        PrintStorage();
        Assert.IsTrue(Store.TokenStore.TryRemoveToken(id, "token"));
        PrintStorage();
        Assert.IsFalse(Store.TokenStore.VerifyToken(id, "token"));
    }

    [TestMethod]
    public void TestRemoveAllTokens()
    {
        var id = Guid.NewGuid();
        Assert.IsTrue(Store.TokenStore.TryAddToken(id, "token1", DateTime.Now.AddDays(1)));
        Assert.IsTrue(Store.TokenStore.TryAddToken(id, "token2", DateTime.Now.AddDays(1)));
        PrintStorage();
        Assert.IsTrue(Store.TokenStore.TryRemoveAllTokens(id));
        PrintStorage();
        Assert.IsFalse(Store.TokenStore.VerifyToken(id, "token1"));
        Assert.IsFalse(Store.TokenStore.VerifyToken(id, "token2"));
    }

    [TestMethod]
    public void TestMutipleUsersAddTokens()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        Assert.IsTrue(Store.TokenStore.TryAddToken(id1, "token1", DateTime.Now.AddDays(1)));
        Assert.IsTrue(Store.TokenStore.TryAddToken(id2, "token2", DateTime.Now.AddDays(1)));
        PrintStorage();
        Assert.IsTrue(Store.TokenStore.VerifyToken(id1, "token1"));
        Assert.IsTrue(Store.TokenStore.VerifyToken(id2, "token2"));
    }

    [TestMethod]
    public void TestMutipleUsersRemoveTokens()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        Assert.IsTrue(Store.TokenStore.TryAddToken(id1, "token1", DateTime.Now.AddDays(1)));
        Assert.IsTrue(Store.TokenStore.TryAddToken(id2, "token2", DateTime.Now.AddDays(1)));
        PrintStorage();
        Assert.IsTrue(Store.TokenStore.TryRemoveToken(id1, "token1"));
        Assert.IsTrue(Store.TokenStore.TryRemoveToken(id2, "token2"));
        PrintStorage();
        Assert.IsFalse(Store.TokenStore.VerifyToken(id1, "token1"));
        Assert.IsFalse(Store.TokenStore.VerifyToken(id2, "token2"));
    }

    [TestMethod]
    public void TestMutipleUsersRemoveAllTokens()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        Assert.IsTrue(Store.TokenStore.TryAddToken(id1, "token1", DateTime.Now.AddDays(1)));
        Assert.IsTrue(Store.TokenStore.TryAddToken(id1, "token2", DateTime.Now.AddDays(1)));
        Assert.IsTrue(Store.TokenStore.TryAddToken(id2, "token3", DateTime.Now.AddDays(1)));
        Assert.IsTrue(Store.TokenStore.TryAddToken(id2, "token4", DateTime.Now.AddDays(1)));
        PrintStorage();
        Assert.IsTrue(Store.TokenStore.TryRemoveAllTokens(id1));
        Assert.IsTrue(Store.TokenStore.TryRemoveAllTokens(id2));
        PrintStorage();
        Assert.IsFalse(Store.TokenStore.VerifyToken(id1, "token1"));
        Assert.IsFalse(Store.TokenStore.VerifyToken(id1, "token2"));
        Assert.IsFalse(Store.TokenStore.VerifyToken(id2, "token3"));
        Assert.IsFalse(Store.TokenStore.VerifyToken(id2, "token4"));
    }

    [TestMethod]
    public void TestGenerateAndAddToken()
    {
        var id = Guid.NewGuid();
        Store.TokenStore.TryGenerateAndAddToken(id, "nickname1", DateTime.Now.AddDays(1), out string token);
        PrintStorage();
        Assert.IsTrue(Store.TokenStore.VerifyToken(id, token));
    }

    [TestMethod]
    public void TestOverlimitToken()
    {
        var id = new Guid();
        for (var i = 0; i < Constants.JWTToken.MaxTokenCount + 1; i++)
        {
            Assert.IsTrue(Store.TokenStore.TryAddToken(id, $"token{i}", DateTime.Now.AddDays(1)));
        }
        PrintStorage();
        Assert.IsFalse(Store.TokenStore.VerifyToken(id, "token0"));
    }

    [TestMethod]
    public void TestExpiredToken()
    {
        var id = Guid.NewGuid();
        Assert.IsTrue(Store.TokenStore.TryAddToken(id, "token", DateTime.Now.AddSeconds(1)));
        Assert.IsTrue(Store.TokenStore.VerifyToken(id, "token"));
        PrintStorage();
        Thread.Sleep(1500);
        Assert.IsFalse(Store.TokenStore.VerifyToken(id, "token"));
        PrintStorage();
    }

    [TestMethod]
    public void TryRemoveNotExistedToken()
    {
        var id = Guid.NewGuid();
        Assert.IsTrue(Store.TokenStore.TryAddToken(id, "token", DateTime.Now.AddDays(1)));
        Assert.IsTrue(Store.TokenStore.TryRemoveToken(id, "token"));
        Assert.IsTrue(Store.TokenStore.TryRemoveToken(id, "token"));

        Assert.IsTrue(Store.TokenStore.TryRemoveToken(new Guid(), "NotExistedToken"));
    }

    [TestMethod]
    public void TryVerifyNotExistedToken()
    {
        var id = Guid.NewGuid();
        Assert.IsFalse(Store.TokenStore.VerifyToken(id, "token"));

        Assert.IsFalse(Store.TokenStore.VerifyToken(new Guid(), "NotExistedToken"));
    }
}
