using System.Linq;
using Domain;
using Domain.Interfaces;
using Domain.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class PolDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public PolDbContext(DbContextOptions<PolDbContext> options, IDomainEventDispatcher dispatcher) : this(options)
        {
            _dispatcher = dispatcher;
        }

        public PolDbContext(DbContextOptions<PolDbContext> options) : base(options)
        {
        }

        public DbSet<TravelExpenseEntity> TravelExpenses { get; set; }

        public override int SaveChanges()
        {
            var result = base.SaveChanges();

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach (var domainEvent in events) _dispatcher?.Dispatch(domainEvent);
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var navigation = modelBuilder.Entity<Guestbook>()
            //    .Metadata.FindNavigation(nameof(Guestbook.Entries));

            //navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}