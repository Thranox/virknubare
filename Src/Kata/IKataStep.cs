using System;
using System.Threading.Tasks;

namespace Kata
{
    public interface IKataStep
    {
        Task ExecuteAndVerifyAsync(Properties properties, string nameOfLoggedInUser, Func<IClientContext, bool> verificationFunc);
        bool CanHandle(string kataStepIdentifier);
    }
}