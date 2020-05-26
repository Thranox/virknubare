using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedWouldBeNugets;

namespace Infrastructure.Data
{
    public class DbSeeder : IDbSeeder
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITravelExpenseFactory _travelExpenseFactory;
        private readonly Dictionary<TravelExpenseStage, string> _dictionary;
        private readonly ILogger _logger;
        private readonly PolDbContext _polDbContext;

        public DbSeeder(IUnitOfWork unitOfWork, ITravelExpenseFactory travelExpenseFactory, ILogger logger, PolDbContext polDbContext)
        {
            _unitOfWork = unitOfWork;
            _travelExpenseFactory = travelExpenseFactory;
            _logger = logger;
            _polDbContext = polDbContext;
            _dictionary = new Dictionary<TravelExpenseStage, string>()
            {
                {TravelExpenseStage.Initial, Globals.InitialReporteddone
                },
                {TravelExpenseStage.ReportedDone, Globals.ReporteddoneCertified
                },
                {TravelExpenseStage.Certified, Globals.CertifiedAssignedForPayment },
                {TravelExpenseStage.AssignedForPayment, Globals.AssignedForPaymentFinal }
            };
        }
        public async Task SeedAsync()
        {
            // -----------------------------------
            // Stages(Not Dummy!)
            foreach (TravelExpenseStage travelExpenseStage in Enum.GetValues(typeof(TravelExpenseStage)))
            {
                GetOrCreateStage(travelExpenseStage);
            }
            await _unitOfWork.CommitAsync();

            // -----------------------------------
            // Dummy customer
            var customer1 = GetOrCreateTestCustomer(TestData.DummyCustomerName1);
            
            // -----------------------------------
            // Dummy Users
            var userEntityPol = GetOrCreateTestUser(customer1, TestData.DummyPolSubAlice, "alice", UserStatus.Registered);
            var userEntitySek = GetOrCreateTestUser(customer1, TestData.DummySekSubBob, "bob", UserStatus.Registered);
            var userEntityLed = GetOrCreateTestUser(customer1, TestData.DummyLedSubCharlie, "charlie", UserStatus.Registered);
            var userEntityAdm = GetOrCreateTestUser(customer1, TestData.DummyAdminSubDennis, "dennis", UserStatus.UserAdministrator);
            var userEntityInit = GetOrCreateTestUser(customer1, TestData.DummyInitialSubEdward, "edward", UserStatus.Initial);



            // -----------------------------------
            // Stages(Not Dummy!)
            var stages=new List<StageEntity>();
            foreach (TravelExpenseStage travelExpenseStage in Enum.GetValues(typeof(TravelExpenseStage)))
            {
                var stageEntity = GetOrCreateStage(travelExpenseStage);
                stages.Add(stageEntity);
                if (travelExpenseStage != TravelExpenseStage.Final)
                {
                   var flowStepEntity = GetOrCreateFlowStep(customer1, stageEntity, TestData.GetFlowStepDescription(travelExpenseStage));
                   GetOrCreateFlowStepUserPermission(flowStepEntity, userEntityPol);

                    if (travelExpenseStage == TravelExpenseStage.ReportedDone)
                    {
                        GetOrCreateFlowStepUserPermission(flowStepEntity, userEntitySek);
                    }

                    if (travelExpenseStage == TravelExpenseStage.Certified)
                    {
                        GetOrCreateFlowStepUserPermission(flowStepEntity, userEntityLed);
                    }
                }
            }

            // -----------------------------------
            // Dummy TravelExpenses
            var polTravelExpenses = _unitOfWork.Repository.List(new TravelExpenseByUserId(userEntityPol.Id));
            if (!polTravelExpenses.Any())
            {
                var travelExpenseDescriptions=Enumerable.Range(1, TestData.GetNumberOfTestDataTravelExpenses()).Select(x=> "Description"+x);
                var newTravelExpenses= travelExpenseDescriptions
                    .Select(x=>_travelExpenseFactory.Create(x, userEntityPol, customer1))
                    .ToArray();
                foreach (var travelExpenseEntity in newTravelExpenses)
                {
                    _unitOfWork.Repository.Add(travelExpenseEntity);
                }

                var travelExpensesToFastForward = newTravelExpenses.Reverse().Take(5);
                foreach (var travelExpenseEntity in travelExpensesToFastForward)
                {
                    travelExpenseEntity.ApplyProcessStep(new FastForwardStep(stages, TravelExpenseStage.AssignedForPayment));
                }

            }

            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveTestDataAsync()
        {
            _logger.Information("Removing testdata from database. Deleting users");

            var polTestUsers = TestData.GetTestUsers();
            foreach (var polTestUser in polTestUsers)
            {
                var userEntity = _unitOfWork
                    .Repository
                    .List(new UserBySub(polTestUser.ImproventoSub.ToString()))
                    .SingleOrDefault();

                if(userEntity==null)
                    continue;

                foreach (var customerUserPermissionEntity in userEntity.CustomerUserPermissions)
                {
                    _unitOfWork.Repository.Delete(customerUserPermissionEntity);
                }

                foreach (var flowStepUserPermissionEntity in userEntity.FlowStepUserPermissions)
                {
                    _unitOfWork.Repository.Delete(flowStepUserPermissionEntity);
                }

                foreach (var travelExpenseEntity in userEntity.TravelExpenses)
                {
                    _unitOfWork.Repository.Delete(travelExpenseEntity);
                }

                _unitOfWork.Repository.Delete(userEntity);
            }

            _logger.Information("Removing testdata from database. Users gone, deleting customers");

            var testCustomers = TestData.GetTestCustomers();
            foreach (var testCustomer in testCustomers)
            {
                var customerEntity = _unitOfWork.Repository.List(new CustomerByName(testCustomer.Name)).SingleOrDefault();

                if(customerEntity==null)
                    continue;

                foreach (var invitationEntity in customerEntity.Invitations)
                {
                    _unitOfWork.Repository.Delete(invitationEntity);
                }

                foreach (var flowStepEntity in customerEntity.FlowSteps)
                {
                    _unitOfWork.Repository.Delete(flowStepEntity);
                }

                _unitOfWork.Repository.Delete(customerEntity);
            }

            _logger.Information("Removing testdata from database. Done. Now committing.");

            await _unitOfWork.CommitAsync();

        }

        public async Task MigrateAsync()
        {
            await _polDbContext.Database.MigrateAsync();
        }

        private CustomerEntity GetOrCreateTestCustomer(string customerName)
        {
           var customer= _unitOfWork.Repository.List(new CustomerByName(customerName)).SingleOrDefault();
            if (customer == null)
            {
                customer = new CustomerEntity(customerName);
                _unitOfWork.Repository.Add(customer);
            }
            else
            {
                Log.Logger.Debug("Dummy Customer already present: "+customerName);
            }

            return customer;
        }

        private object GetOrCreateFlowStepUserPermission(FlowStepEntity flowStepEntity, UserEntity userEntity)
        {
            var flowStepUserPermissionEntity = _unitOfWork
                .Repository
                .List(new FlowStepUserPermissionByFlowStepAndUser(flowStepEntity, userEntity))
                .SingleOrDefault();
            if (flowStepUserPermissionEntity == null)
            {
                flowStepUserPermissionEntity=new FlowStepUserPermissionEntity(flowStepEntity, userEntity);
                _unitOfWork.Repository.Add(flowStepUserPermissionEntity);
            }

            return flowStepUserPermissionEntity;
        }

        private UserEntity GetOrCreateTestUser(CustomerEntity customer, string sub, string userName, UserStatus userStatus)
        {
            var user =_unitOfWork
                .Repository
                .List(new UserBySub(sub)) 
                .SingleOrDefault();

            if (user == null)
            {
                user = new UserEntity(userName, sub);
                var claims = TestData.GetClaimsByUserName(userName);
                user.UpdateWithClaims(claims);

                _unitOfWork.Repository.Add(user);
                customer.AddUser(user, userStatus);
            }

            return user;
        }
        
        private StageEntity GetOrCreateStage(TravelExpenseStage travelExpenseStage)
        {
            var stageEntity =_unitOfWork.Repository.List(new StageByValue(travelExpenseStage)).SingleOrDefault();
            if (stageEntity == null)
            {
                stageEntity = new StageEntity(travelExpenseStage);
                _unitOfWork.Repository.Add(stageEntity);
            }

            return stageEntity;
        }
        
        private FlowStepEntity GetOrCreateFlowStep(CustomerEntity customer, StageEntity stage, string description)
        {
            var flowStepEntity =_unitOfWork.Repository.List(new FlowStepByCustomerAndStage(customer.Id, (TravelExpenseStage) stage.Value)).SingleOrDefault();
            if (flowStepEntity == null)
            {
                var key = _dictionary[stage.Value];
                flowStepEntity = new FlowStepEntity(key, stage, customer, description);
                _unitOfWork.Repository.Add(flowStepEntity);
            }

            return flowStepEntity;
        }

        /// <summary>
        /// This ProcessFlowStep is used for 'cheating' test data into desired state. This should never be made available outside DbSeeder.
        /// </summary>
        private class FastForwardStep : IProcessFlowStep
        {
            private readonly List<StageEntity> _stages;
            private readonly TravelExpenseStage _travelExpenseStage;

            public FastForwardStep(List<StageEntity> stages, TravelExpenseStage travelExpenseStage)
            {
                _stages = stages;
                _travelExpenseStage = travelExpenseStage;
            }
            public bool CanHandle(string key)
            {
                return true;
            }

            public StageEntity GetResultingStage(TravelExpenseEntity travelExpenseEntity)
            {
                return _stages.Single(x => x.Value == _travelExpenseStage);
            }
        }
    }
}