namespace Domain.Entities
{
    public class UserEntity
    {
        public CustomerEntity Customer { get; set; }
        public string Subject { get; set; }
    }
}