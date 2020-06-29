using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Domain.SharedKernel;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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

        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<StageEntity> Stages { get; set; }
        public DbSet<SubmissionEntity> Submissions { get; set; }
        public DbSet<EmailEntity> Emails { get; set; }
        public DbSet<LossOfEarningSpecEntity> LossOfEarningSpecs { get; set; }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach (var domainEvent in events)
                {
                    if (_dispatcher != null)
                    {
                        await _dispatcher.Dispatch(domainEvent);
                    }
                }
            }

            return result;
        }

        public override int SaveChanges()
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlowStepUserPermissionEntity>()
                .HasKey(bc => new {bc.FlowStepId, bc.UserId});

            modelBuilder.Entity<FlowStepUserPermissionEntity>()
                .HasOne(bc => bc.FlowStep)
                .WithMany(b => b.FlowStepUserPermissions)
                .HasForeignKey(bc => bc.FlowStepId);
            modelBuilder.Entity<FlowStepUserPermissionEntity>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.FlowStepUserPermissions)
                .HasForeignKey(bc => bc.UserId);

            modelBuilder.Entity<EmailEntity>()
                .Property(e => e.Recievers)
                .HasConversion(
                v => string.Join(';', v),
                v => v.Split(';', StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<CustomerEntity>().ToTable("Customers");
            modelBuilder.Entity<UserEntity>().ToTable("Users");
            modelBuilder.Entity<FlowStepEntity>().ToTable("FlowSteps");
            modelBuilder.Entity<FlowStepUserPermissionEntity>().ToTable("FlowStepUserPermissions");
            modelBuilder.Entity<TravelExpenseEntity>().ToTable("TravelExpenses");
            modelBuilder.Entity<TravelExpenseEntity>()
                .Property(x => x.DailyAllowanceAmount)
                .HasConversion(
                    xx => JsonConvert.SerializeObject(xx),
                    xxx => JsonConvert.DeserializeObject<DailyAllowanceAmount>(xxx));
            modelBuilder.Entity<TravelExpenseEntity>()
                .Property(x => x.FoodAllowances)
                .HasConversion(
                    xx => JsonConvert.SerializeObject(xx),
                    xxx => JsonConvert.DeserializeObject<FoodAllowances>(xxx));
            modelBuilder.Entity<TravelExpenseEntity>()
                .Property(x => x.DestinationPlace)
                .HasConversion(
                    xx => JsonConvert.SerializeObject(xx),
                    xxx => JsonConvert.DeserializeObject<Place>(xxx));
            modelBuilder.Entity<TravelExpenseEntity>()
                .Property(x => x.ArrivalPlace)
                .HasConversion(
                    xx => JsonConvert.SerializeObject(xx),
                    xxx => JsonConvert.DeserializeObject<Place>(xxx));
            modelBuilder.Entity<TravelExpenseEntity>()
                .Property(x => x.DeparturePlace)
                .HasConversion(
                    xx => JsonConvert.SerializeObject(xx),
                    xxx => JsonConvert.DeserializeObject<Place>(xxx));
            modelBuilder.Entity<TravelExpenseEntity>()
                .Property(x => x.TransportSpecification)
                .HasConversion(
                    xx => JsonConvert.SerializeObject(xx),
                    xxx => JsonConvert.DeserializeObject<TransportSpecification>(xxx));

            modelBuilder.Entity<CustomerUserPermissionEntity>().ToTable("CustomerUserPermissions");
            modelBuilder.Entity<InvitationEntity>().ToTable("Invitations");
            modelBuilder.Entity<SubmissionEntity>().ToTable("Submissions");
            modelBuilder.Entity<EmailEntity>().ToTable("Emails");
            modelBuilder.Entity<LossOfEarningEntity>().ToTable("LossOfEarnings");
        }
    }
}