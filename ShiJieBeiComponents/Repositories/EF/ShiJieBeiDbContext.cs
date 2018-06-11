using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.SqlServer;
using System.Net.Mime;
using log4net;
using ShiJieBeiComponents.Domains;

namespace ShiJieBeiComponents.Repositories.EF
{
    public class ShiJieBeiDbConfiguration : DbConfiguration
    {
        public ShiJieBeiDbConfiguration()
        {
            this.SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy(3, TimeSpan.FromSeconds(10)));
        }
    }
    [DbConfigurationType(typeof(ShiJieBeiDbConfiguration))]
    public class ShiJieBeiDbContext : DbContext
    {
        protected static ILog logger = LogManager.GetLogger(typeof(ShiJieBeiDbContext));
        private string guid = Guid.NewGuid().ToString();
        public string InstanceId
        {
            get
            {
                return guid;
            }
        }
        public ShiJieBeiDbContext()
            : base("ShiJieBeiDb")
        {
        }
        public ShiJieBeiDbContext(String connString)
            : base(connString)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);




            modelBuilder.Configurations.Add(new UserMapping());
            modelBuilder.Configurations.Add(new AccountMapping());
            modelBuilder.Configurations.Add(new AccountCashOutLogMapping());

        }
    }

    public class UserMapping : EntityTypeConfiguration<User>
    {
        public UserMapping()
        {
            ToTable("Users");
        }
    }

    public class AccountMapping : EntityTypeConfiguration<Account>
    {
        public AccountMapping()
        {
            ToTable("Accounts");
            HasKey(a => a.Id)
                .HasRequired(a => a.User)
                .WithRequiredDependent(u => u.Account)
                .WillCascadeOnDelete(true);

            HasMany(a => a.MoneyLogs)
                .WithRequired(l => l.Account)
                .HasForeignKey(l => l.AccountId)
                .WillCascadeOnDelete(true);

            HasMany(a => a.CashOutLogs)
                .WithRequired(l => l.Account)
                .HasForeignKey(l => l.AccountId)
                .WillCascadeOnDelete(true);

            HasMany(a => a.VouchersLogs)
                .WithRequired(l => l.Account)
                .HasForeignKey(l => l.AccountId)
                .WillCascadeOnDelete(true);

            HasMany(a => a.ChargeLogs)
                .WithRequired(l => l.Account)
                .HasForeignKey(l => l.AccountId)
                .WillCascadeOnDelete(true);
        }
    }

    public class AccountCashOutLogMapping : EntityTypeConfiguration<AccountCashOutLog>
    {
        public AccountCashOutLogMapping()
        {
            ToTable("AccountCashOutLogs");

            HasMany(a => a.OpLogs)
                .WithRequired(l => l.CashOut)
                .HasForeignKey(l => l.CashOutId)
                .WillCascadeOnDelete(true);
        }
    }
}