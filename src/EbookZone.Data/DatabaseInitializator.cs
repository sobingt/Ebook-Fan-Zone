using System.Data.Entity;
using EbookZone.Domain;
using EbookZone.Domain.Base;
using EbookZone.Domain.Enums;
using EbookZone.Utils.Helpers;

namespace EbookZone.Data
{
    public class DatabaseInitializator<T> : DropCreateDatabaseIfModelChanges<EFZDataContext<T>> where T : BaseEntity
    {
        protected override void Seed(EFZDataContext<T> context)
        {
            if(typeof(T) == typeof(User))
            {
                var adminUser = new User
                                    {
                                        AccountType = AccountType.Default,
                                        UserType = UserType.Administrator,
                                        Email = "admin@ebook-fan-zone.com",
                                        FirstName = "Administrator",
                                        LastName = "",
                                        Password = EncryptionHelper.Encrypt("admin@ebook-fan-zone.com", "P@ssw0rd")
                                    };

                context.Entities.Add(adminUser as T);
                context.SaveChanges();
            }

            base.Seed(context);
        }
    }
}