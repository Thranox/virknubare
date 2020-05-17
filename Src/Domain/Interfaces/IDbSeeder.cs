using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDbSeeder
    {
        void Seed();
        Task RemoveTestData();
    }
}