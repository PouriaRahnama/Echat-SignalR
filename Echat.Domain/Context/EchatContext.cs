using System.Linq;
using Echat.Domain.Entities.Chats;
using Echat.Domain.Entities.Roles;
using Echat.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Echat.Domain.Context
{
    public class EchatContext : DbContext
    {
        public EchatContext(DbContextOptions<EchatContext> options) : base(options)
        {

        }

        #region Entities
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>()
                .HasOne(b => b.User)
                .WithMany(b => b.Chats)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserGroup>()
                .HasOne(b => b.User)
                .WithMany(b => b.UserGroups)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);
        }
    }
}