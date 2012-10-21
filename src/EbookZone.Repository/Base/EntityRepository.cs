﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using EbookZone.Data;
using EbookZone.Domain;
using EbookZone.Domain.Base;

namespace EbookZone.Repository.Base
{
    public class EntityRepository<T> : IEntityRepository<T> where T : BaseEntity
    {
        #region Implementation of IEntityRepository<T>

        public void Create(T entity)
        {
            using (DataContext<T> context = new DataContext<T>())
            {
                context.Entities.Add(entity);
                context.SaveChanges();
            }
        }

        public IList<T> Load()
        {
            using (DataContext<T> context = new DataContext<T>())
            {
                return context.Entities.ToList();
            }
        }

        public T Load(int id)
        {
            using (DataContext<T> context = new DataContext<T>())
            {
                T entity = context.Entities.Find(id);
                return entity;
            }
        }

        public void Update(T entity)
        {
            using (DataContext<T> context = new DataContext<T>())
            {
                context.Entities.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Delete(T entity)
        {
            using (DataContext<T> context = new DataContext<T>())
            {
                context.Entities.Attach(entity);
                context.Entry(entity).State = EntityState.Deleted;
                context.Entities.Remove(entity);
                context.SaveChanges();
            }
        }

        #endregion Implementation of IEntityRepository<T>
    }
}