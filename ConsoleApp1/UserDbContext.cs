using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    using System.Data.Entity;

    class BaseContext<TContext> : DbContext where TContext : DbContext
    {
        static BaseContext()
        {
            Database.SetInitializer<TContext>(null);
        }

        protected BaseContext() : base("UserContext")
        {

        }
    }

    class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletInfo> WalletInfoes { get; set; }

        public DatabaseContext() : base("UserContext")
        {
        }
    }

    class UserDbContext : BaseContext<UserDbContext>
    {
        public DbSet<User> Users { get; set; }
    }

    class WalletDbContext : BaseContext<WalletDbContext>
    {
        public DbSet<Wallet> Wallets { get; set; }
    }

    class WalletInfoDbContext : BaseContext<WalletInfoDbContext>
    {
        public DbSet<WalletInfo> WalletInfos { get; set; }
    }
}
