using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Domain.ValueObjects;
using Serilog;
using SharedWouldBeNugets;

namespace Infrastructure.Data
{
    public class DbSeeder : IDbSeeder
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITravelExpenseFactory _travelExpenseFactory;
        private readonly Dictionary<TravelExpenseStage, string> _dictionary;

        public DbSeeder(IUnitOfWork unitOfWork, ITravelExpenseFactory travelExpenseFactory)
        {
            _unitOfWork = unitOfWork;
            _travelExpenseFactory = travelExpenseFactory;
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
            CustomerEntity customer=null;
            foreach (var testCustomer in TestData.GetTestCustomers())
            {
                customer = _unitOfWork.Repository.List(new CustomerByName(testCustomer.Name)).SingleOrDefault();
                if (customer == null)
                {
                    customer = new CustomerEntity(testCustomer.Name);
                    _unitOfWork.Repository.Add(customer);
                }
                else
                {
                    Log.Logger.Debug("Dummy Customer already present");
                }
            }

            // -----------------------------------
            // Dummy Users
            var userEntityPol = GetOrCreateTestUser(customer, TestData.DummyPolSubAlice, "dummy pol Alice", UserStatus.Registered);
            var userEntitySek = GetOrCreateTestUser(customer, TestData.DummySekSubBob, "dummy sek Bob", UserStatus.Registered);
            var userEntityLed = GetOrCreateTestUser(customer, TestData.DummyLedSubCharlie, "dummy led Charlie", UserStatus.Registered);


            // -----------------------------------
            // Stages(Not Dummy!)
            foreach (TravelExpenseStage travelExpenseStage in Enum.GetValues(typeof(TravelExpenseStage)))
            {
                var stageEntity = GetOrCreateStage(travelExpenseStage);
                if (travelExpenseStage != TravelExpenseStage.Final)
                {
                    var flowStepEntity = GetOrCreateFlowStep(customer, stageEntity, TestData.GetFlowStepDescription(travelExpenseStage));
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
                _unitOfWork.Repository.Add(_travelExpenseFactory.Create("Description1", userEntityPol, customer));
                _unitOfWork.Repository.Add(_travelExpenseFactory.Create("Description2", userEntityPol, customer));
                _unitOfWork.Repository.Add(_travelExpenseFactory.Create("Description3", userEntityPol, customer));
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveTestDataAsync()
        {
            var polTestUsers = TestData.GetTestUsers();
            foreach (var polTestUser in polTestUsers)
            {
                var userEntity = _unitOfWork.Repository.List<UserEntity>(new UserBySub(polTestUser.ImproventoSub.ToString())).SingleOrDefault();
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

            var testCustomers = TestData.GetTestCustomers();
            foreach (var testCustomer in testCustomers)
            {
                var customerEntity = _unitOfWork.Repository.List(new CustomerByName(testCustomer.Name)).SingleOrDefault();

                if(customerEntity==null)
                    continue;

                foreach (var flowStepEntity in customerEntity.FlowSteps)
                {
                    _unitOfWork.Repository.Delete(flowStepEntity);
                }

                _unitOfWork.Repository.Delete(customerEntity);
            }

            await _unitOfWork.CommitAsync();

            await Task.CompletedTask;
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
                var key = _dictionary[(TravelExpenseStage)stage.Value];
                flowStepEntity = new FlowStepEntity(key, stage, customer, description);
                _unitOfWork.Repository.Add(flowStepEntity);
            }

            return flowStepEntity;
        }
    }

}