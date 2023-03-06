using AuthorizationService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.DAL.Context
{
    public class AuthServiceDbContext : DbContext
    {
        public AuthServiceDbContext(DbContextOptions options) : base(options)
        {
    #region Начальные данные для теста

            if (Database.EnsureCreated())
            {
                Users.Add(new User
                {
                    Login = "Anoshkin99",
                    Password = "$MYHASH$V1$10000$kytEmmOsV7hAInKeXv3j6/wisVRxRDWqRzYSYX81AdfehXED",   //denis12345
                    Group = new UserGroup
                    {
                        Name = "admin",
                        Description = "Админская группа",
                        Permissions = new List<Permission> { new Permission { Name = "Админское право", Action = "выдача прав другим пользователям" } }

                    }
                });
                SaveChanges();
            }

     #endregion  Начальные данные для теста
        }

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<UserGroup> UserGroups { get; set; } = null!;

        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        public DbSet<Permission> Permissions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(ent => ent.HasAlternateKey(user => user.Login));
        }
    }
}
