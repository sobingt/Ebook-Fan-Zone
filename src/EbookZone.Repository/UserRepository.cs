using EbookZone.Domain;
using EbookZone.Repository.Base;

namespace EbookZone.Repository
{
    public class UserRepository : EntityRepository<User>
    {
        public static User Load(string email, string password)
        {
            return null;
        }
    }
}