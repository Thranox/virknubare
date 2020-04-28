using System;
using Domain.SharedKernel;

namespace Domain.Entities
{
    public class FlowStepUserPermissionEntity : BaseEntity
    {
        private FlowStepUserPermissionEntity()
        {
        }

        public FlowStepUserPermissionEntity(FlowStepEntity flowStep, UserEntity user):this()
        {
            FlowStep = flowStep;
            User = user;
        }

        public Guid FlowStepId { get; private set;}
        public FlowStepEntity FlowStep { get; set; }
        public Guid UserId { get; private set; }
        public UserEntity User { get; set; }
    }
}