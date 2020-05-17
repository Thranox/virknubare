using System.Threading.Tasks;

namespace Kata
{
    public interface IKataStep
    {
        Task ExecuteAsync(Properties properties);
        bool CanHandle(string kataStepIdentifier);
    }
}