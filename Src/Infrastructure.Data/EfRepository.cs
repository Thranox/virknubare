using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Domain.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class EfRepository : IRepository
    {
        private readonly PolDbContext _dbContext;

        public EfRepository(PolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetById<T>(Guid id) where T : BaseEntity
        {
            //if (typeof(T) == typeof(Guestbook))
            //{
            //    return _dbContext.Set<Guestbook>().Include(g => g.Entries).SingleOrDefault(e => e.Id == id) as T;
            //}
            return _dbContext.Set<T>().SingleOrDefault(e => e.Id == id);
        }

        public List<T> List<T>(ISpecification<T> spec = null) where T : BaseEntity
        {
            if (typeof(T) == typeof(InvitationEntity))
            {
                IQueryable<InvitationEntity> includableQueryable = _dbContext
                    .Set<InvitationEntity>()
                    .Include(g => g.Customer);

                if (spec != null)
                {
                    var castedSpec = spec as ISpecification<InvitationEntity>;
                    includableQueryable = includableQueryable.Where(castedSpec.Criteria);
                }

                return includableQueryable.ToList() as List<T>;
            }

            if (typeof(T) == typeof(CustomerEntity))
            {
                IQueryable<CustomerEntity> includableQueryable = _dbContext
                    .Set<CustomerEntity>()
                    .Include(g => g.FlowSteps).ThenInclude(gg => gg.FlowStepUserPermissions)
                    .Include(g => g.TravelExpenses).ThenInclude(gg => gg.Stage)
                    .Include(g => g.CustomerUserPermissions).ThenInclude(gg => gg.User)
                    .Include(g => g.Invitations)
                    .Include(g=>g.LossOfEarningSpecs);

                if (spec != null)
                {
                    var castedSpec = spec as ISpecification<CustomerEntity>;
                    includableQueryable = includableQueryable.Where(castedSpec.Criteria);
                }

                return includableQueryable.ToList() as List<T>;
            }

            if (typeof(T) == typeof(TravelExpenseEntity))
            {
                IQueryable<TravelExpenseEntity> includableQueryable = _dbContext
                    .Set<TravelExpenseEntity>()
                    .Include(g => g.Stage)
                    .Include(g => g.OwnedByUser);

                if (spec != null)
                {
                    var castedSpec = spec as ISpecification<TravelExpenseEntity>;
                    includableQueryable = includableQueryable.Where(castedSpec.Criteria);
                }

                return includableQueryable.ToList() as List<T>;
            }

            if (typeof(T) == typeof(UserEntity))
            {
                IQueryable<UserEntity> includableQueryable = _dbContext
                        .Set<UserEntity>()
                        .Include(g => g.FlowStepUserPermissions)
                        .ThenInclude(gg => gg.FlowStep)
                        .ThenInclude(ggg => ggg.From)
                        .Include(g => g.CustomerUserPermissions)
                        .ThenInclude(gg => gg.Customer)
                        .ThenInclude(ggg=>ggg.LossOfEarningSpecs)
                        .Include(g => g.TravelExpenses)
                        .ThenInclude(gg => gg.Stage )
                        .Include(g=>g.TravelExpenses)
                        .ThenInclude(gg=>gg.LossOfEarningEntities)
                    ;
                if (spec != null)
                {
                    var castedSpec = spec as ISpecification<UserEntity>;
                    includableQueryable = includableQueryable.Where(castedSpec.Criteria);
                }

                return includableQueryable.ToList() as List<T>;
            }

            if (typeof(T) == typeof(FlowStepEntity))
            {
                IQueryable<FlowStepEntity> includableQueryable = _dbContext
                        .Set<FlowStepEntity>()
                        .Include(g => g.FlowStepUserPermissions)
                        .ThenInclude(gg => gg.FlowStep)
                        .ThenInclude(ggg => ggg.From)
                        .Include(g => g.From)
                    ;
                if (spec != null)
                {
                    var castedSpec = spec as ISpecification<FlowStepEntity>;
                    includableQueryable = includableQueryable.Where(castedSpec.Criteria);
                }

                return includableQueryable.ToList() as List<T>;
            }

            if (typeof(T) == typeof(SubmissionEntity))
            {
                IQueryable<SubmissionEntity> includableQueryable = _dbContext
                    .Set<SubmissionEntity>()
                    .Include(g => g.Customer);

                if (spec != null)
                {
                    var castedSpec = spec as ISpecification<SubmissionEntity>;
                    includableQueryable = includableQueryable.Where(castedSpec.Criteria);
                }

                return includableQueryable.ToList() as List<T>;
            }

            var query = _dbContext.Set<T>().AsQueryable();
            if (spec != null) query = query.Where(spec.Criteria);
            return query.ToList();
        }

        public T Add<T>(T entity) where T : BaseEntity
        {
            var baseEntity = entity as BaseEntity;
            if (baseEntity.Id != Guid.Empty)
                throw new ArgumentException("Id can't be set on entity before calling Add (is this already added?)");

            baseEntity.Id = Guid.NewGuid();

            _dbContext.Set<T>().Add(entity);

            return entity;
        }

        public void Delete<T>(T entity) where T : BaseEntity
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Attach<T>(T entity) where T : BaseEntity
        {
            _dbContext.Attach(entity);
        }

        public void Update<T>(T entity) where T : BaseEntity
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}