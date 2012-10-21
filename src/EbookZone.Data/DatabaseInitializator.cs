using System.Data.Entity;
using EbookZone.Domain;
using EbookZone.Domain.Base;

namespace EbookZone.Data
{
    public class DatabaseInitializator<T> : DropCreateDatabaseIfModelChanges<DataContext<T>> where T : BaseEntity
    {
        protected override void Seed(DataContext<T> context)
        {
            // Items which creates after database creation on Server

            base.Seed(context);
        }
    }
}