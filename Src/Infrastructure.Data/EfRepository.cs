﻿using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SharedKernel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data;

namespace CleanArchitecture.Infrastructure.Data
{
    public class EfRepository : IRepository
    {
        private readonly PolDbContext _dbContext;

        public EfRepository(PolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetById<T>(int id) where T : BaseEntity
        {
            //if (typeof(T) == typeof(Guestbook))
            //{
            //    return _dbContext.Set<Guestbook>().Include(g => g.Entries).SingleOrDefault(e => e.Id == id) as T;
            //}
            return _dbContext.Set<T>().SingleOrDefault(e => e.Id == id);
        }

        public List<T> List<T>(ISpecification<T> spec = null) where T : BaseEntity
        {
            //if (typeof(T) == typeof(Guestbook))
            //{
            //    return _dbContext.Set<Guestbook>().Include(g => g.Entries).ToList() as List<T>;
            //}
            var query = _dbContext.Set<T>().AsQueryable();
            if (spec != null)
            {
                query = query.Where(spec.Criteria);
            }
            return query.ToList();
        }

        public T Add<T>(T entity) where T : BaseEntity
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public void Delete<T>(T entity) where T : BaseEntity
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Update<T>(T entity) where T : BaseEntity
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}