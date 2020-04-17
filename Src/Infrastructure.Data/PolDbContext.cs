using System.Linq;
using Domain;
using Domain.Entities;
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
        public DbSet<CustomerEntity> CustomerEntities { get; set; }

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
        }

        public void Seed()
        {
            if (CustomerEntities.Any(x => x.Name == Globals.DummyCustomerName))
                return;

            var customerEntity = new CustomerEntity(Globals.DummyCustomerName);
            var userEntityPol = new UserEntity("dummy pol", Globals.DummyPolSub);
            customerEntity.Users.Add(userEntityPol);
            var userEntitySek = new UserEntity("dummy sek", "123451");
            customerEntity.Users.Add(userEntitySek);
            var userEntityLed = new UserEntity("dummy led", "123452");
            customerEntity.Users.Add(userEntityLed);

            var flowStepEntity1 = new FlowStepEntity(Globals.InitialReporteddone, TravelExpenseStage.Initial);
            flowStepEntity1.FlowStepUserPermissions.Add(
                new FlowStepUserPermissionEntity {FlowStep = flowStepEntity1, User = userEntityPol}
            );
            customerEntity.FlowSteps.Add(flowStepEntity1);

            var flowStepEntity2 = new FlowStepEntity(Globals.ReporteddoneCertified, TravelExpenseStage.ReportedDone);
            flowStepEntity2.FlowStepUserPermissions.Add(
                new FlowStepUserPermissionEntity {FlowStep = flowStepEntity2, User = userEntitySek}
            );
            customerEntity.FlowSteps.Add(flowStepEntity2);

            var flowStepEntity3 = new FlowStepEntity(Globals.CertifiedAssignedForPayment, TravelExpenseStage.Certified);
            flowStepEntity3.FlowStepUserPermissions.Add(
                new FlowStepUserPermissionEntity {FlowStep = flowStepEntity3, User = userEntityLed}
            );
            customerEntity.FlowSteps.Add(flowStepEntity3);

            var flowStepEntity4 =
                new FlowStepEntity(Globals.AssignedForPaymentFinal, TravelExpenseStage.AssignedForPayment);
            flowStepEntity4.FlowStepUserPermissions.Add(
                new FlowStepUserPermissionEntity {FlowStep = flowStepEntity4, User = userEntityLed}
            );
            customerEntity.FlowSteps.Add(flowStepEntity4);

            customerEntity.TravelExpenses.Add(new TravelExpenseEntity("Description1", userEntityPol));
            customerEntity.TravelExpenses.Add(new TravelExpenseEntity("Description2", userEntityPol));
            customerEntity.TravelExpenses.Add(new TravelExpenseEntity("Description3", userEntityPol));

            CustomerEntities.Add(customerEntity);

            SaveChanges();
        }
    }
}