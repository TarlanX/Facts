using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Calabonga.EntityFrameworkCore.Entities.Base;
using Calabonga.UnitOfWork;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Teagle.Facts.Web.Data.Base
{
    public abstract class DbContextBase : IdentityDbContext
    {
        public SaveChangesResult SaveChangesResult { get; set; }
        protected DbContextBase(DbContextOptions options)
            : base(options)
        {
            SaveChangesResult = new SaveChangesResult();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(Startup).Assembly);
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            DbSaveChanges();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            DbSaveChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            DbSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            DbSaveChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void DbSaveChanges()
        {
            //Added 
            const string defaultUser = "System";
            var defaultDate = DateTime.UtcNow;

            var addedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);

            foreach (var entry in addedEntities)
            {
                if (entry.Entity is not IAuditable)
                {
                    continue;
                }

                var createAt = entry.Property(nameof(IAuditable.CreatedAt)).CurrentValue;
                var updateAt = entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue;
                var createBy = entry.Property(nameof(IAuditable.CreatedBy)).CurrentValue;
                var updateBy = entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue;

                if (string.IsNullOrEmpty(createBy?.ToString()))
                {
                    entry.Property(nameof(IAuditable.CreatedBy)).CurrentValue = defaultUser;
                }

                if (string.IsNullOrEmpty(updateBy?.ToString()))
                {
                    entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue = defaultUser;
                }

                if (DateTime.Parse(createAt?.ToString()!).Year < 1970)
                {
                    entry.Property(nameof(IAuditable.CreatedAt)).CurrentValue = defaultDate;
                }

                if (updateAt != null && DateTime.Parse(updateAt.ToString()!).Year < 1970)
                {
                    entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = defaultDate;
                }
                else
                {
                    entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = defaultDate;
                }

                SaveChangesResult.AddMessage("Some entities were created");

            }

            // Modified

            var modifiedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);

            foreach (var entry in modifiedEntities)
            {
                if (entry.Entity is IAuditable)
                {
                    var userName = entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue == null
                        ? defaultUser
                        : entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue;
                    entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = defaultDate;
                    entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue = userName;
                }
            }
        }
    }
}