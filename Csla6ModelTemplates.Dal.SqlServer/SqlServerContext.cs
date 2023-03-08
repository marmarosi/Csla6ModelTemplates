using Csla6ModelTemplates.Dal.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Csla6ModelTemplates.Dal.SqlServer
{
    /// <summary>
    /// Represents a session with the database.
    /// </summary>
    public class SqlServerContext : DbContext //DbContextBase
    {
        #region Constructors

        public SqlServerContext(
            DbContextOptions<SqlServerContext> options
            )
            : base(options)
        { }

        #endregion

        #region Auto update timestamps

        void SubscribeStateChangeEvents()
        {
            ChangeTracker.Tracked += OnEntityTracked;
            ChangeTracker.StateChanged += OnEntityStateChanged;
        }

        void OnEntityTracked(object sender, EntityTrackedEventArgs e)
        {
            if (!e.FromQuery)
                ProcessLastModified(e.Entry);
        }

        void OnEntityStateChanged(object sender, EntityStateChangedEventArgs e)
        {
            ProcessLastModified(e.Entry);
        }

        void ProcessLastModified(EntityEntry entry)
        {
            if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
            {
                var property = entry.Metadata.FindProperty("Timestamp");
                if (property != null && property.ClrType == typeof(DateTime))
                    entry.CurrentValues[property] = DateTime.UtcNow;
            }
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
            modelBuilder.Entity<Team>()
                .Property(e => e.Timestamp)
                .HasDefaultValue(DateTime.Now);

            #endregion

            #region Player

            modelBuilder.Entity<Player>()
                .HasIndex(e => new { e.TeamKey, e.PlayerCode })
                .IsUnique();

            #endregion

            #region Folder

            modelBuilder.Entity<Folder>()
                .HasIndex(e => new { e.ParentKey, e.FolderOrder });
            modelBuilder.Entity<Folder>()
                .Property(e => e.Timestamp)
                .HasDefaultValue(DateTime.Now);

            #endregion

            #region Group

            modelBuilder.Entity<Group>()
                .HasIndex(e => e.GroupCode)
                .IsUnique();
            modelBuilder.Entity<Group>()
                .Property(e => e.Timestamp)
                .HasDefaultValue(DateTime.Now);

            #endregion

            #region Person

            modelBuilder.Entity<Person>()
                .HasIndex(e => e.PersonCode)
                .IsUnique();
            modelBuilder.Entity<Person>()
                .Property(e => e.Timestamp)
                .HasDefaultValue(DateTime.Now);

            #endregion

            #region GroupPerson

            modelBuilder.Entity<GroupPerson>()
                .HasKey(e => new { e.GroupKey, e.PersonKey })
                .IsClustered();

            #endregion
        }
    }
}
