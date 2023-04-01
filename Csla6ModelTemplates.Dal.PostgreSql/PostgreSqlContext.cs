using Csla6ModelTemplates.Dal.PostgreSql.Entities;
using Microsoft.EntityFrameworkCore;

namespace Csla6ModelTemplates.Dal.PostgreSql
{
    /// <summary>
    /// Represents a session with the database.
    /// </summary>
    public class PostgreSqlContext : DbContext, ITransactionOptions
    {
        #region Constructors

        public bool IsUnderTest { get; private set; }

        public PostgreSqlContext(
            DbContextOptions<PostgreSqlContext> options,
            ITransactionOptions transactionOptions
            )
            : base(options)
        {
            IsUnderTest = transactionOptions?.IsUnderTest ?? false;
        }

        #endregion

        #region Auto update timestamps

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var insertedEntries = this.ChangeTracker.Entries()
                               .Where(x => x.State == EntityState.Added)
                               .Select(x => x.Entity);

            foreach (var insertedEntry in insertedEntries)
            {
                var auditableEntity = insertedEntry as Timestamped;
                //If the inserted object is an Auditable. 
                if (auditableEntity != null)
                {
                    auditableEntity.Timestamp = DateTimeOffset.UtcNow;
                }
            }

            var modifiedEntries = this.ChangeTracker.Entries()
                       .Where(x => x.State == EntityState.Modified)
                       .Select(x => x.Entity);

            foreach (var modifiedEntry in modifiedEntries)
            {
                //If the inserted object is an Auditable. 
                var auditableEntity = modifiedEntry as Timestamped;
                if (auditableEntity != null)
                {
                    auditableEntity.Timestamp = DateTimeOffset.UtcNow;
                }
            }

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        #endregion

        #region Query results

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<GroupPerson> GroupPersons { get; set; }

        #endregion

        /// <summary>
        /// Configure the model discovered by convention from the entity type..
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(
            ModelBuilder modelBuilder
            )
        {
            #region Team

            modelBuilder.Entity<Team>()
                .HasIndex(e => e.TeamCode)
                .IsUnique();

            #endregion

            #region Player

            modelBuilder.Entity<Player>()
                .HasIndex(e => new { e.TeamKey, e.PlayerCode })
                .IsUnique();

            #endregion

            #region Folder

            modelBuilder.Entity<Folder>()
                .HasIndex(e => new { e.ParentKey, e.FolderOrder });

            #endregion

            #region Group

            modelBuilder.Entity<Group>()
                .HasIndex(e => e.GroupCode)
                .IsUnique();

            #endregion

            #region Person

            modelBuilder.Entity<Person>()
                .HasIndex(e => e.PersonCode)
                .IsUnique();

            #endregion

            #region GroupPerson

            modelBuilder.Entity<GroupPerson>()
                .HasKey(e => new { e.GroupKey, e.PersonKey });

            #endregion
        }
    }
}
