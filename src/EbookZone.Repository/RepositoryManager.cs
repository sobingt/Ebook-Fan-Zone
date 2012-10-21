namespace EbookZone.Repository
{
    public class RepositoryManager
    {
        public static UserRepository Users
        {
            get { return new UserRepository(); }
        }
    }
}