using Domain.SharedKernel;

namespace Domain.Entities
{
    public class InvitationEntity : BaseEntity
    {
        public string Email { get; set; }

        private InvitationEntity()
        {
            
        }
        public InvitationEntity(string email):this()
        {
            Email = email;
        }
    }
}