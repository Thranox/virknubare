using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Specifications
{
    public class FlowStepUserPermissionByFlowStepAndUser : ISpecification<FlowStepUserPermissionEntity>
    {
        public FlowStepUserPermissionByFlowStepAndUser(FlowStepEntity flowStepEntity, UserEntity userEntity)
        {
            Criteria = e => e.FlowStep==flowStepEntity && e.User==userEntity;
        }

        public Expression<Func<FlowStepUserPermissionEntity, bool>> Criteria { get; }
    }
}