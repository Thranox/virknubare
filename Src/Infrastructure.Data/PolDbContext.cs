using System.Linq;
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

        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<StageEntity> Stages { get; set; }

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

            modelBuilder.Entity<CustomerEntity>().ToTable("Customers");
            modelBuilder.Entity<UserEntity>().ToTable("Users");
            modelBuilder.Entity<FlowStepEntity>().ToTable("FlowSteps");
            modelBuilder.Entity<FlowStepUserPermissionEntity>().ToTable("FlowStepUserPermissions");
            modelBuilder.Entity<TravelExpenseEntity>().ToTable("TravelExpenses");
            modelBuilder.Entity<CustomerUserPermissionEntity>().ToTable("CustomerUserPermissions");


        }

        //public void Seed()
        //{
        //    if (Customers.Any(x => x.Name == TestData.DummyCustomerName))
        //        return;

        //    var customerEntity = new CustomerEntity(TestData.DummyCustomerName);
        //    var userEntityPol = new UserEntity("dummy pol Alice", TestData.DummyPolSubAlice);
        //    customerEntity.Users.Add(userEntityPol);
        //    var userEntitySek = new UserEntity("dummy sek Bob", TestData.DummySekSubBob);
        //    customerEntity.Users.Add(userEntitySek);
        //    var userEntityLed = new UserEntity("dummy led Charlie", TestData.DummyLedSubCharlie);
        //    customerEntity.Users.Add(userEntityLed);

        //    var flowStepEntity1 = new FlowStepEntity(Globals.InitialReporteddone, TravelExpenseStage.Initial);
        //    flowStepEntity1.FlowStepUserPermissions.Add(
        //        new FlowStepUserPermissionEntity {FlowStep = flowStepEntity1, User = userEntityPol}
        //    );
        //    customerEntity.FlowSteps.Add(flowStepEntity1);

        //    var flowStepEntity2 = new FlowStepEntity(Globals.ReporteddoneCertified, TravelExpenseStage.ReportedDone);
        //    flowStepEntity2.FlowStepUserPermissions.Add(
        //        new FlowStepUserPermissionEntity {FlowStep = flowStepEntity2, User = userEntitySek}
        //    );
        //    customerEntity.FlowSteps.Add(flowStepEntity2);

        //    var flowStepEntity3 = new FlowStepEntity(Globals.CertifiedAssignedForPayment, TravelExpenseStage.Certified);
        //    flowStepEntity3.FlowStepUserPermissions.Add(
        //        new FlowStepUserPermissionEntity {FlowStep = flowStepEntity3, User = userEntityLed}
        //    );
        //    customerEntity.FlowSteps.Add(flowStepEntity3);

        //    var flowStepEntity4 =
        //        new FlowStepEntity(Globals.AssignedForPaymentFinal, TravelExpenseStage.AssignedForPayment);
        //    flowStepEntity4.FlowStepUserPermissions.Add(
        //        new FlowStepUserPermissionEntity {FlowStep = flowStepEntity4, User = userEntityLed}
        //    );
        //    customerEntity.FlowSteps.Add(flowStepEntity4);

        //    customerEntity.TravelExpenses.Add(new TravelExpenseEntity("Description1", userEntityPol));
        //    customerEntity.TravelExpenses.Add(new TravelExpenseEntity("Description2", userEntityPol));
        //    customerEntity.TravelExpenses.Add(new TravelExpenseEntity("Description3", userEntityPol));

        //    Customers.Add(customerEntity);

        //    SaveChanges();
        //}
    }
}