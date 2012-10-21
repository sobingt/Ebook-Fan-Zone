using System.Data.Entity;
using EbookZone.Domain;
using EbookZone.Domain.Base;

namespace EbookZone.Data
{
    public class DatabaseInitializator<T> : DropCreateDatabaseIfModelChanges<EFZDataContext<T>> where T : BaseEntity
    {
        protected override void Seed(EFZDataContext<T> context)
        {
            // Items which creates after database creation on Server

            base.Seed(context);
        }
    }
}