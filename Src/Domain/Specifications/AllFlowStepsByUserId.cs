﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class CustomerById : ISpecification<CustomerEntity>
    {
        public CustomerById(Guid customerId)
        {
            Criteria = e => e.Id== customerId;
        }

        public Expression<Func<CustomerEntity, bool>> Criteria { get; }
    }
    public class AllFlowStepsByUserId: ISpecification<FlowStepEntity>
    {
        public AllFlowStepsByUserId(Guid userId)
        {
            Criteria = e => e.FlowStepUserPermissions.Any(x=>x.UserId==userId);
        }

        public Expression<Func<FlowStepEntity, bool>> Criteria { get; }
    }
}