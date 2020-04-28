using System;
using Domain.SharedKernel;

namespace Domain.Entities
{
    public class CustomerUserPermissionEntity : BaseEntity
    {
        public Guid CustomerId { get; private set; }
        public CustomerEntity Customer { get; set; }
        public Guid UserId { get; private set; }
        public UserEntity User { get; set; }
        public UserStatus UserStatus { get; set; }
    }
}