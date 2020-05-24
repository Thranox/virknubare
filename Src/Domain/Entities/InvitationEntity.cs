using System;
using Domain.SharedKernel;

namespace Domain.Entities
{
    public class InvitationEntity : BaseEntity
    {
        public DateTime CreationTime { get; set; }
        public string Email { get; set; }

        private InvitationEntity()
        {
            
        }
        public InvitationEntity(string email):this()
        {
            Email = email;
            CreationTime=DateTime.Now;
        }
    }
}