using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using EbookZone.Domain;

namespace EbookZone.Utils.Helpers
{
    public static class EFAttributeExt
    {
        public static string GetTableName<T>(this DbContext context) where T : BaseEntity
        {
            var type = typeof(T);

            var tableAttribute = type.GetCustomAttributes(false).OfType<TableAttribute>().FirstOrDefault();

            if (tableAttribute != null)
                return tableAttribute.Name;

            return string.Empty;
        }
    }
}