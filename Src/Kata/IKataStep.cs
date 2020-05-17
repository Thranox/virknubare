using System.Threading.Tasks;

namespace Kata
{
    public interface IKataStep
    {
        Task ExecuteAsync(Properties properties, string nameOfLoggedInUser);
        bool CanHandle(string kataStepIdentifier);
    }
}