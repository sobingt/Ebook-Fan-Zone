using System.Data.Entity;
using EbookZone.Domain.Base;
using EbookZone.Utils.Constants;
using EbookZone.Utils.Helpers;

namespace EbookZone.Data
{
    public class EFZDataContext<T> : DbContext where T : BaseEntity
    {
        public static string ConnString = ApplicationConfig.ConnStringSetting;

        public DbSet<T> Entities { get; set; }

        public EFZDataContext()
            : base(ConnString)
        {
            //Database.SetInitializer(new DatabaseInitializator<T>());
            Database.SetInitializer(new CreateDatabaseIfNotExists<EFZDataContext<T>>());

            Database.Initialize(true);

            Configuration.AutoDetectChangesEnabled = false;

            var exist = Database.Exists();

            if (!exist)
            {
                Database.Create();
            }

            Database.Connection.Open();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T>().Map(m => m.MapInheritedProperties()).ToTable(this.GetTableName<T>());
        }
    }
}