using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbookZone.Domain;

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