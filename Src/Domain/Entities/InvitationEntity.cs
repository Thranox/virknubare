using System;
using Domain.SharedKernel;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class InvitationEntity : BaseEntity
    {
        public DateTime CreationTime { get; set; }
        public string Email { get; set; }
        public InvitationState InvitationState { get; set; }

        private InvitationEntity()
        {
            
        }
        public InvitationEntity(string email):this()
        {
            Email = email;
            CreationTime=DateTime.Now;
            InvitationState = InvitationState.Invited;
        }
    }
}