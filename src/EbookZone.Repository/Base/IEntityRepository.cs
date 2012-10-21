using System.Collections.Generic;
using EbookZone.Domain.Base;

namespace EbookZone.Repository.Base
{
    public interface IEntityRepository<T> where T : BaseEntity
    {
        void Create(T entity);

        IList<T> Load();

        T Load(int id);

        void Update(T entity);

        void Delete(T entity);
    }
}