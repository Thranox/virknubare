using Domain.SharedKernel;

namespace Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        private UserEntity()
        {
        }

        public UserEntity(string name, string subject) : this()
        {
            Name = name;
            Subject = subject;
        }

        public CustomerEntity Customer { get; set; }
        public string Name { get; }
        public string Subject { get; }
    }
}