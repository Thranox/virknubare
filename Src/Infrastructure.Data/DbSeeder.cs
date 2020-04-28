using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Serilog;
using SharedWouldBeNugets;

namespace Infrastructure.Data
{
    public class DbSeeder : IDbSeeder
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Dictionary<TravelExpenseStage, string> _dictionary;

        public DbSeeder(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public void Seed()
        {
            // -----------------------------------
            // Dummy customer
            var customer =_unitOfWork.Repository.List(new CustomerByName(TestData.DummyCustomerName)).SingleOrDefault();
            if (customer == null)
            {
                customer = new CustomerEntity(TestData.DummyCustomerName);
                _unitOfWork.Repository.Add(customer);
            }
            else
            {
                Log.Logger.Debug("Dummy Customer already present");
            }
            
            // -----------------------------------
            // Dummy Users
            var userEntityPol = GetOrCreateTestUser(customer, TestData.DummyPolSubAlice, "dummy pol Alice", UserStatus.Registered);
            var userEntitySek = GetOrCreateTestUser(customer, TestData.DummySekSubBob, "dummy sek Bob", UserStatus.Registered);
            var userEntityLed = GetOrCreateTestUser(customer, TestData.DummyLedSubCharlie, "dummy led Charlie", UserStatus.Registered);


            // Stages (Not Dummy!)
            //foreach (TravelExpenseStage travelExpenseStage in Enum.GetValues(typeof(TravelExpenseStage)))
            //{
            //    var stageEntity = GetOrCreateStage(travelExpenseStage);
            //    if (travelExpenseStage != TravelExpenseStage.Final)
            //    {
            //        var flowStepEntity = GetOrCreateFlowStep(customer, stageEntity);
            //        flowStepEntity.AddUserPermission(userEntityPol);
            //    }
            //}

            _unitOfWork.Commit();
        }


        private UserEntity GetOrCreateTestUser(CustomerEntity customer, string sub, string userName, UserStatus userStatus)
        {
            var user =_unitOfWork
                .Repository
                .List(new UserBySubSpecification(sub)) 
                .SingleOrDefault();

            if (user == null)
            {
                user = new UserEntity(userName, sub);
                _unitOfWork.Repository.Add(user);
                customer.AddUser(user, userStatus);
            }

            return user;
        }
    }
}