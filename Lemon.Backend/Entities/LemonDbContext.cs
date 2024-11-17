using Microsoft.EntityFrameworkCore;

namespace Lemon.Backend.Entities;

public class LemonDbContext(DbContextOptions<LemonDbContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplySeedData();
    }
}

public static class ModelBuilderExtensions
{
    public static void ApplySeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "John",
                PhoneNumber = "12345678901",
                NickName = "John Ave",
                PasswordHash = [0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A],
                Salt = [0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A],
            },
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "Robert",
                Email = "Robert@mail.com",
                PhoneNumber = "12345678902",
                NickName = "Robert St",
                PasswordHash = [0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A],
                Salt = [0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A],
            }
        );
    }
}
