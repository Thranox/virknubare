using System;
using System.Collections.Generic;
using System.Linq;
using Domain.SharedKernel;

namespace Domain.Entities
{
    public class FlowStepUserPermissionEntity : BaseEntity
    {
        public Guid FlowStepId { get; private set;}
        public FlowStepEntity FlowStep { get; set; }
        public Guid UserId { get; private set; }
        public UserEntity User { get; set; }

        //private FlowStepUserPermissionEntity()
        //{
        //}

        //public FlowStepUserPermissionEntity(Guid flowStepId, Guid userId) : this()
        //{
        //    FlowStepId = flowStepId;
        //    UserId = userId;
        //}
    }
}